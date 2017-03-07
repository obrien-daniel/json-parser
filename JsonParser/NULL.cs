/// <summary>
/// NULL class used to handle JSON null values.
/// </summary>
namespace JSONParser
{
    class NULL : BaseObject
    {
        public override string Print(int indent)
        {
            //string TAB_INDENT = "";
            //for (int i = 0; i < indent; i++)
            //    TAB_INDENT += "\t";
            return "null";
        }
        public override int GetWeight()
        {
            return 1;
        }
    }
}
