using System.IO;
using System.Text.RegularExpressions;

namespace AutoBrowser.Library
{
    public static class File
    {
        #region Functions
        public static void WriteOnFile(string text, string fileFullName)
        {
            var textFile = new FileInfo(fileFullName);

            if (string.IsNullOrEmpty(textFile.Extension))
            {
                textFile = new FileInfo(fileFullName + ".txt");
            }

            System.IO.File.AppendAllText(textFile.FullName, $"{text}\n");
        }
        #endregion

        #region Validations
        public static string FormatValidFileName(string fileFullName)
        {
            string validNameFile = RemoveInvalidCharacters(fileFullName);
            return TrimValidLength(validNameFile);
        }

        public static string TrimValidLength(string fileFullName)
        {
            if (fileFullName.Length > 259)
            {
                string extencion = fileFullName.Substring(fileFullName.LastIndexOf('.'));
                string fileName = fileFullName.Substring(fileFullName.LastIndexOf(@"\")+1);
                string path = fileFullName.Substring(0, fileFullName.LastIndexOf(@"\"));

                int folderNameLength = new DirectoryInfo(path).FullName.Length;
                int extencionLength = extencion.Length;

                fileName = fileName.Substring(0, 250 - (folderNameLength + extencionLength)).Trim();

                fileFullName = Path.Combine(new DirectoryInfo(path).FullName, (fileName + extencion));
            }

            return fileFullName;
        }

        public static string RemoveInvalidCharacters(string fileFullName)
        {
            string fileName = fileFullName.Substring(fileFullName.LastIndexOf(@"\")+1);
            string path = fileFullName.Substring(0, fileFullName.LastIndexOf(@"\"));

            return Path.Combine(RemoveInvalidCharactersPath(path), RemoveInvalidCharactersFile(fileName));
        }

        public static string RemoveInvalidCharactersFile(string fileName)
        {
            fileName = Regex.Replace(fileName, @"\s{2,}", " ");

            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(fileName, invalidRegStr, "");
        }

        public static string RemoveInvalidCharactersPath(string path)
        {
            path = Regex.Replace(path, @"\s{2,}", " ");

            string invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return Regex.Replace(path, invalidRegStr, "");
        }
        #endregion
    }
}
