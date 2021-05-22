using System.IO;

namespace AutoBrowser.Actions
{
    public class WriteFile
    {

        public static void WriteOnFile(string fileName, string text)
        {
            fileName += fileName.EndsWith(".txt") ? "" : ".txt";
            File.AppendAllText(fileName, $"{text}\n");
        }
    }
}
