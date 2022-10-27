using JsonParser;

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
    string line = File.ReadAllText(args[0]); // TODO: currently just grabs whole text file, this needs to change for reading larger files, possibly using BufferedStream and streamreader.
    var jsonObject = Parser.Parse(line, ref index);
    if (jsonObject == null)
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
        Console.WriteLine("Weight of tree (amount of values): " + jsonObject.GetWeight());
        bool flag = false;

        Console.Write("Would you like to pretty print this in the console? [y/n] ");
        do
        {
            var answer = Console.ReadKey(true).Key;

            if (answer == ConsoleKey.Y)
            {
                Console.WriteLine(jsonObject.Print(0));
                flag = true;
                Console.WriteLine();
            }
            else if (answer == ConsoleKey.N)
            {
                flag = true;
                Console.WriteLine();
            }
        } while (!flag);

        flag = false;
        Console.Write("Would you like to save the pretty printed JSON to a local file? [y/n] ");
        do
        {
            var answer = Console.ReadKey(true).Key;

            if (answer == ConsoleKey.Y)
            {
                File.WriteAllText("./pretty_json.json", jsonObject.Print(0));
                Console.WriteLine("./pretty_json.json saved sucessfully.");
                flag = true;
                Console.WriteLine();
            }
            else if (answer == ConsoleKey.N)
            {
                flag = true;
                Console.WriteLine();
            }
        } while (!flag);
    }

    Console.WriteLine();
    Console.Write("Press any key to exit...");
    Console.ReadKey();
}