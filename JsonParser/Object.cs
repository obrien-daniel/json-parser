using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Object class to handle object JSON value. 
/// </summary>
/// 
namespace JSONParser
{
    class Object : BaseObject
    {
        public Dictionary<string, BaseObject> Value = new Dictionary<string, BaseObject>();

        public override string Print(int indent)
        {
            if (Value.Count == 0)
                return "{}";
            string TAB_INDENT = "";
            for (int i = 0; i < indent; i++)
                TAB_INDENT += "\t";
            string result = "\n" + TAB_INDENT + "{\n";
            indent++;

            foreach (var item in Value)
                result += TAB_INDENT+"\t\""+item.Key + "\": " + @item.Value.Print(indent) + (item.Key != Value.Last().Key ? "," : "") + "\n";
            result += TAB_INDENT + "}";
            return result;
        }
        public override int GetWeight()
        {
            int count = 1;
            foreach (var item in Value)
                count += item.Value.GetWeight();
            return count;
        }
    }
}
