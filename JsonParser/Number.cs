/// <summary>
/// Number class used as a data structure to handle a number that is not formatted.
/// </summary>

namespace JSONParser
{
    internal class Number : BaseObject
    {
        public Number(double value) // Constructor
        {
            Value = value;
        }

        public double Value { get; private set; } // Getter and Setter for value property

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