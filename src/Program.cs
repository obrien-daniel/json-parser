using System;
using System.IO;

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
    public class Program
    {
        /// <summary>
        /// Parses json file. The user can view the amount of values in the JSON file and pretty print it.
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: JsonParse.exe [path-to-json-file]");
                Console.WriteLine();
                Console.WriteLine("path-to-json-file:");
                Console.WriteLine("\tThe path to a .json file to validate.");
                Console.WriteLine();
                Console.Write("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            else
            {
                int index = 0;
                BaseObject root;
                string line = File.ReadAllText(args[0]); // TODO: currently just grabs whole text file, this needs to change for reading larger files, possibly using BufferedStream and streamreader.
                root = Parser.Parse(line, ref index);
                if (root == null)
                {
                    Console.WriteLine();
                    Console.WriteLine("The JSON supplied is ill-formed.");
                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The JSON supplied is well-formed.");
                    Console.WriteLine();
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
    }
}