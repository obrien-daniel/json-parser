/// <summary>
/// String class used to handle JSON unformatted string values.
/// </summary>
namespace JSONParser
{
    public class String : BaseObject
    {
        public String(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public override string Print(int indent)
        {
            string value = Value.Replace("\\", "\\\\").Replace("\r", "\\r").Replace("\n", "\\n").Replace("\"", "\\\"");
            return "\"" + value + "\"";
        }

        public override int GetWeight()
        {
            return 1;
        }
    }
}