/// <summary>
/// Base class used for JSON Values
/// </summary>

namespace JsonParser
{
    public abstract class BaseObject
    {
        public abstract string Print(int indent);

        public abstract int GetWeight();
    }
}