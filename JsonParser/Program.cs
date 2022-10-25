using System;
using System.IO;
using System.Text;

/// <summary>
/// This Program parses JSON into a tree of values using inheritance. The amount of values in the document are printed to the console, and
/// the user can view a printy print of the json file by either console or file.
///
/// Object-Oriented Programming HW 2
/// 3-7-17
/// by Daniel O'Brien
/// </summary>
namespace JSONParser
{
    internal class Program
    {
        /// <summary>
        /// Parses json file. The user can view the amount of values in the JSON file and pretty print it.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            if (args.Length != 1) // If using VS 2015, set this in Project->Properties->Debug->Command-line arguments
            {
                Console.WriteLine("Invalid argument length.");
                Console.WriteLine("Usage: JsonParse.exe [json file]");
                Console.Write("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                int index = 0;
                BaseObject root;
                string line = File.ReadAllText(args[0]); //currently just grabs whole text file, this needs to change for reading larger files, possibly using BufferedStream and streamreader.
                root = Parse(line, ref index);
                if (root == null)
                {
                    Console.WriteLine("The JSON supplied is ill-formed.");
                }
                else
                {
                    Console.WriteLine("The JSON supplied is well-formed.");
                    Console.WriteLine("Weight of tree (amount of values): " + root.GetWeight());
                    bool flag = false;
                    // Check if user wants to pretty print json
                    do
                    {
                        Console.WriteLine("Would you like to pretty print this in the console? (y/n)");
                        string answer = Console.ReadLine();
                        if (answer.ToLower().Equals("y"))
                        {
                            Console.WriteLine(root.Print(0));
                            flag = true;
                        }
                        else if (answer.ToLower().Equals("n"))
                        {
                            flag = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                    } while (!flag);
                    // Check if user wants to save to a local file
                    flag = false;
                    do
                    {
                        Console.WriteLine("Would you like to save the pretty printed JSON to a local file? (y/n)");
                        string answer = Console.ReadLine();
                        if (answer.ToLower().Equals("y"))
                        {
                            File.WriteAllText("./pretty_json.json", root.Print(0));
                            Console.WriteLine("./pretty_json.json saved sucessfully.");
                            flag = true;
                        }
                        else if (answer.ToLower().Equals("n"))
                        {
                            flag = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input.");
                        }
                    } while (!flag);
                }
                // Wait for input to exit
                Console.Write("Press any key to exit...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Main parse method that determines which JSON value type to parse.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static BaseObject Parse(string value, ref int index)
        {
            //   while(index < value.Length) //while within string
            //  {
            char currentValue = value[index];
            SkipWhitespace(value, ref index);
            if (currentValue == '\\')
            { //escape character
                index++;
                currentValue = value[index];
            }

            return currentValue switch
            {
                't' => ParseTrue(ref index),
                'f' => ParseFalse(ref index),
                'n' => ParseNULL(ref index),
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
        public static bool IsNumerical(char c)
        {
            return "0123456789-".IndexOf(c) != -1;
        }

        /// <summary>
        /// Checks if value[index] is contained within " \t\n\r", which means it's whitespace. If not, it returns
        /// a -1 and breaks out of the loop.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public static void SkipWhitespace(string value, ref int index)
        {
            for (; index < value.Length; index++)
            {
                if (" \t\n\r".IndexOf(value[index]) == -1)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Parse a number into a Number data structure.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static BaseObject ParseNumber(string value, ref int index)
        {
            SkipWhitespace(value, ref index);
            int end = GetLastIndexOfNumber(value, index);
            int length = end - index;
            if (double.TryParse(value.Substring(index, length), out double number))
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
        /// Get the index of the last number + 1
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        protected static int GetLastIndexOfNumber(string value, int index)
        {
            int end;
            for (end = index; end < value.Length; end++)
            {
                if ("0123456789.+-eE".IndexOf(value[end]) == -1) // finds where the index isn't one part of "0123456789.+-eE"
                {
                    break;
                }
            }

            return end;
        }

        /// <summary>
        /// Parse boolean true into Bool data structure
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static BaseObject ParseTrue(ref int index)
        {
            index += 4; // true is 4 characters
            return new Bool(true);
        }

        /// <summary>
        /// Parse boolean false into Bool data structure
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static BaseObject ParseFalse(ref int index)
        {
            index += 5; // false is 5 characters
            return new Bool(false);
        }

        /// <summary>
        /// Parse NULL into the NULL data structure
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static BaseObject ParseNULL(ref int index)
        {
            index += 4; // Skil rest of NULL characters
            return new NULL();
        }

        /// <summary>
        /// Parse string into the String datastructure. Escape characters are preserved.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static BaseObject ParseString(string value, ref int index)
        {
            StringBuilder str = new StringBuilder();
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
            return new JSONParser.String(str.ToString());
        }

        /// <summary>
        /// Parse strings into string. This is used for object key names.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string ParseObjectStringName(string value, ref int index)
        {
            StringBuilder str = new StringBuilder();

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
        public static BaseObject ParseObject(string value, ref int index)
        {
            SkipWhitespace(value, ref index);
            index++; // skip {
            SkipWhitespace(value, ref index);
            Object obj = new JSONParser.Object(); // new object
            //char previousChar = '\0';
            while (value[index] != '}')  // && previousChar != '\\')
            {
                SkipWhitespace(value, ref index);
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
            }
            index++; //skip }
            return obj;
        }

        /// <summary>
        /// Parse JSON array into Array data structure
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static BaseObject ParseArray(string value, ref int index)
        {
            SkipWhitespace(value, ref index);
            index++; // skip [
            SkipWhitespace(value, ref index);
            Array array = new JSONParser.Array();
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