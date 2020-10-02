/// <summary>
/// String class used to handle JSON unformatted string values.
/// </summary>
namespace JSONParser
{
    internal class String : BaseObject
    {
        public String(string value) // Constructor
        {
            Value = value;
        }
        public string Value { get; private set; } // Getter and Setter for value property

        public override string Print(int indent)
        {

            //string TAB_INDENT = "";
            //for (int i = 0; i < indent; i++)
            //    TAB_INDENT += "\t";
            string value = Value.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            return "\"" + value + "\"";
        }
        public override int GetWeight()
        {
            return 1;
        }
    }
}
