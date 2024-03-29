﻿/// <summary>
/// NULL class used to handle JSON null values.
/// </summary>
namespace JsonParser
{
    public class Null : BaseObject
    {
        public override string Print(int indent)
        {
            return "null";
        }

        public override int GetWeight()
        {
            return 1;
        }
    }
}