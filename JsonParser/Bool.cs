/// <summary>
/// Bool class used to handle TRUE and FALSE JSON values.
/// </summary>
namespace JSONParser
{
    internal class Bool : BaseObject
    {
        public Bool(bool value)
        {
            Value = value;
        }

        public bool Value { get; private set; }

        public override string Print(int indent)
        {
            return Value.ToString().ToLower();
        }

        public override int GetWeight()
        {
            return 1;
        }
    }
}