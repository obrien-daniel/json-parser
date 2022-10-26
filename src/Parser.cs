using System;
using System.Text;

namespace JSONParser
{
    public static class Parser
    {
        /// <summary>
        /// Main parse method that recursively parses a string into a JSON object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static BaseObject Parse(string value, ref int index)
        {
            char currentValue = value[index];
            SkipWhitespace(value, ref index);
            if (currentValue == '\\') //escape character
            {
                index++;
                currentValue = value[index];
            }

            return currentValue switch
            {
                't' => ParseTrue(ref index),
                'f' => ParseFalse(ref index),
                'n' => ParseNull(ref index),
                '{' => ParseObject(value, ref index),
                '[' => ParseArray(value, ref index),
                '"' => ParseString(value, ref index),
                var _ when IsNumerical(currentValue) => ParseNumber(value, ref index),
                _ => null,
            };
        }

        /// <summary>
        /// Determines if character is considered a numerical character for JSON. This is
        /// only applied to the first character to determine if the program should parse a number.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static bool IsNumerical(char c)
        {
            return "0123456789-".IndexOf(c) != -1;
        }

        /// <summary>
        /// Skips characters that are whitespace. When a non-whitespace character is found, the loop is broken, so the index is at the non-whitespace character.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        private static void SkipWhitespace(string value, ref int index)
        {
            for (; index < value.Length; index++)
            {
                if (!" \t\n\r\b\f\v".Contains(value[index])) break;
            }
        }

        /// <summary>
        /// Parse a number into a Number object.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static BaseObject ParseNumber(string value, ref int index)
        {
            SkipWhitespace(value, ref index);
            int end = GetNextNonNumberIndex(value, index);
            int length = end - index;
            if (double.TryParse(value.AsSpan(index, length), out double number))
            {
                index = end;
                return new Number(number);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Get the index of the next character that's not a number.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static int GetNextNonNumberIndex(string value, int index)
        {
            int end;
            for (end = index; end < value.Length; end++)
            {
                if (!"0123456789.+-eE".Contains(value[end]))
                {
                    break;
                }
            }

            return end;
        }

        /// <summary>
        /// Parse boolean true into Bool object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static BaseObject ParseTrue(ref int index)
        {
            index += 4; // true is 4 characters
            return new Bool(true);
        }

        /// <summary>
        /// Parse boolean false into Bool object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static BaseObject ParseFalse(ref int index)
        {
            index += 5; // false is 5 characters
            return new Bool(false);
        }

        /// <summary>
        /// Parse NULL into the NULL object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static BaseObject ParseNull(ref int index)
        {
            index += 4; // Skil rest of NULL characters
            return new Null();
        }

        /// <summary>
        /// Parse string into the String object. Escape characters are preserved.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static BaseObject ParseString(string value, ref int index)
        {
            var str = new StringBuilder();
            SkipWhitespace(value, ref index);
            _ = value[index++]; // skip "
            bool complete = false;
            while (!complete)
            {
                if (index == value.Length) break;  // if at end

                char currentChar = value[index++];
                if (currentChar == '"')
                {
                    break;
                }
                else if (currentChar == '\\') // if at escape character
                {
                    if (index == value.Length) break;

                    currentChar = value[index++];
                    if (currentChar == '"')
                    {
                        str.Append('"');
                    }
                    else if (currentChar == '/')
                    {
                        str.Append('/');
                    }
                    else if (currentChar == '\\')
                    {
                        str.Append('\\');
                    }
                    else if (currentChar == 'r') // return
                    {
                        str.Append('\r');
                    }
                    else if (currentChar == 'n') //new line
                    {
                        str.Append('\n');
                    }
                    else if (currentChar == 't') // tab
                    {
                        str.Append('\t');
                    }
                    else if (currentChar == 'a') // alert
                    {
                        str.Append('\a');
                    }
                    else if (currentChar == 'b') // backspace
                    {
                        str.Append('\b');
                    }
                    else if (currentChar == 'f') // form feed
                    {
                        str.Append('\f');
                    }
                    else if (currentChar == 'f') // vertical tab - probably not used
                    {
                        str.Append('\f');
                    }
                }
                else
                {
                    str.Append(currentChar);
                }
            }
            return new String(str.ToString());
        }

        /// <summary>
        /// Parse strings into string. This is used for object key names.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static string ParseObjectStringName(string value, ref int index)
        {
            StringBuilder str = new();

            SkipWhitespace(value, ref index);
            _ = value[index++]; // "
            bool complete = false;
            while (!complete)
            {
                if (index == value.Length) break; // if at end

                char currentChar = value[index++];
                if (currentChar == '"')
                {
                    break;
                }
                else if (currentChar == '\\') // if at escape character
                {
                    if (index == value.Length) break;

                    currentChar = value[index++];
                    if (currentChar == '"')
                    {
                        str.Append('"');
                    }
                    else if (currentChar == '/')
                    {
                        str.Append('/');
                    }
                    else if (currentChar == '\\')
                    {
                        str.Append('\\');
                    }
                    else if (currentChar == 'r') // return
                    {
                        str.Append('\r');
                    }
                    else if (currentChar == 'n') //new line
                    {
                        str.Append('\n');
                    }
                    else if (currentChar == 't') // tab
                    {
                        str.Append('\t');
                    }
                    else if (currentChar == 'a') // alert
                    {
                        str.Append('\a');
                    }
                    else if (currentChar == 'b') // backspace
                    {
                        str.Append('\b');
                    }
                    else if (currentChar == 'f') // form feed
                    {
                        str.Append('\f');
                    }
                    else if (currentChar == 'f') // vertical tab - probably not used
                    {
                        str.Append('\f');
                    }
                }
                else
                {
                    str.Append(currentChar);
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// Parse object that begins with { and ends with }.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static BaseObject ParseObject(string value, ref int index)
        {
            SkipWhitespace(value, ref index);
            index++; // skip {
            SkipWhitespace(value, ref index);
            var obj = new Object(); // new object
            //char previousChar = '\0';
            while (value[index] != '}')
            {
                if (value[index] == ',')
                {
                    index++;
                }

                SkipWhitespace(value, ref index);
                string name = ParseObjectStringName(value, ref index);
                SkipWhitespace(value, ref index);
                index++; // skip :
                SkipWhitespace(value, ref index);
                BaseObject bobj = Parse(value, ref index); // parse value
                if (bobj == null)
                    return null;
                obj.Value.Add(name, bobj);

                SkipWhitespace(value, ref index);
            }
            index++; //skip }
            return obj;
        }

        /// <summary>
        /// Parse JSON array into Array object
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private static BaseObject ParseArray(string value, ref int index)
        {
            SkipWhitespace(value, ref index);
            index++; // skip [
            SkipWhitespace(value, ref index);
            var array = new Array();
            while (value[index] != ']')
            {
                SkipWhitespace(value, ref index);
                BaseObject obj = Parse(value, ref index);
                if (obj == null)
                    return null;
                array.List.Add(obj);
                SkipWhitespace(value, ref index);
                if (value[index] == ',')
                    index++; // skip comma
                SkipWhitespace(value, ref index);
                if (index == value.Length) // if at end
                    break;
            }
            index++; //skip ]
            return array;
        }
    }
}