﻿using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Array class used to handle JSON array values.
/// </summary>
namespace JsonParser
{
    public class Array : BaseObject
    {
        public List<BaseObject> List = new();

        public override string Print(int indent)
        {
            if (List.Count == 0)
            {
                return "[]";
            }

            string TAB_INDENT = "";
            for (int i = 0; i < indent; i++)
            {
                TAB_INDENT += "\t";
            }

            string result = "\n" + TAB_INDENT + "[\n";
            indent++;
            foreach (BaseObject obj in List)
            {
                result += @obj.Print(indent) + (obj != List.Last() ? "," : "") + "\n";
            }

            result += TAB_INDENT + "]";
            return result;
        }

        public override int GetWeight()
        {
            int count = 1;
            foreach (BaseObject obj in List)
            {
                count += obj.GetWeight();
            }
            return count;
        }
    }
}