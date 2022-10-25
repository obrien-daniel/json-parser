/// <summary>
/// Bool class used to handle TRUE and FALSE JSON values.
/// </summary>
namespace JSONParser
{
    internal class Bool : BaseObject
    {
        public Bool(bool value) // Constructor
        {
            Value = value;
        }

        public bool Value { get; private set; } // Getter and Setter for value property

        public override string Print(int indent)
        {
            //string TAB_INDENT = "";
            //for (int i = 0; i < indent; i++)
            //    TAB_INDENT += "\t";
            return Value.ToString().ToLower();
        }

        public override int GetWeight()
        {
            return 1;
        }
    }
}