namespace AdventOfCode
{
    public class FileUtil
    {
        public delegate void ParseLine(string line);

        public static void Parse(int day, ParseLine parseLineDelegate)
        {
            string line;
            System.IO.StreamReader file = System.IO.File.OpenText("inputs/day" + day + ".txt");
            while ((line = file.ReadLine()) != null)
            {
                parseLineDelegate(line);
            }

            file.Dispose();
        }
    }
}
