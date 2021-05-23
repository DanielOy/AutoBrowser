using System.IO;

namespace AutoBrowser.Actions
{
    public class WriteFile
    {

        public static void WriteOnFile(string text, string fileFullName)
        {
            var textFile = new FileInfo(fileFullName);

            if (string.IsNullOrEmpty(textFile.Extension))
            {
                textFile = new FileInfo(fileFullName + ".txt");
            }

            File.AppendAllText(textFile.FullName, $"{text}\n");
        }
    }
}
