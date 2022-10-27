/// <summary>
/// Number class used as a data structure to handle a number that is not formatted.
/// </summary>

namespace JsonParser
{
    public class Number : BaseObject
    {
        public Number(double value)
        {
            Value = value;
        }

        public double Value { get; private set; }

        public override string Print(int indent)
        {
            return Value.ToString();
        }

        public override int GetWeight()
        {
            return 1;
        }
    }
}