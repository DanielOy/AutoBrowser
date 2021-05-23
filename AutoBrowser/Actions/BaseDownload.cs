using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public abstract class BaseDownload : WebAction
    {
        #region Properties
        public string DownloadFolder { get; set; } = Path.Combine(Application.StartupPath, "Downloads");
        public string FileName { get; set; }
        public string Url { get; set; }
        #endregion

        #region Variables
        protected string _originalfileName;
        protected string _originalDownloadFolder;
        protected string _originalUrl;
        #endregion

        #region Constructor
        public BaseDownload(string url, string fileName)
        {
            Url = _originalUrl = url;
            FileName = _originalfileName = fileName;
            DownloadFolder = _originalDownloadFolder = DownloadFolder;
        }

        public BaseDownload(string url, string fileName, string downloadFolder)
        {
            Url = _originalUrl = url;
            FileName = _originalfileName = fileName;
            DownloadFolder = _originalDownloadFolder = downloadFolder;
        }
        #endregion

        #region Functions
        public override void ReplaceVariables(Dictionary<string, object> savedValues)
        {
            if (savedValues == null)
            {
                return;
            }

            ResetValues();

            foreach (var item in savedValues)
            {
                if (Url.Contains($"[{item.Key}]"))
                {
                    Url = Url.Replace($"[{item.Key}]", item.Value.ToString());
                }

                if (DownloadFolder.Contains($"[{item.Key}]"))
                {
                    DownloadFolder = DownloadFolder.Replace($"[{item.Key}]", item.Value.ToString());
                }

                if (FileName.Contains($"[{item.Key}]"))
                {
                    FileName = FileName.Replace($"[{item.Key}]", item.Value.ToString());
                }
            }
        }

        protected override void ResetValues()
        {
            Url = _originalUrl;
            FileName = _originalfileName;
            DownloadFolder = _originalDownloadFolder;
        }

        protected FileInfo GetValidFileInfo()
        {
            if (!Directory.Exists(DownloadFolder))
            {
                Directory.CreateDirectory(DownloadFolder);
            }

            RemoveInvalidCharacters();

            string FullPath = Path.Combine(new DirectoryInfo(DownloadFolder).FullName, FileName);

            FullPath = FormatValidLength(FullPath);

            return new FileInfo(FullPath);
        }

        private string FormatValidLength(string FullPath) //TODO: Extract in a library class
        {
            if (FullPath.Length > 259)
            {
                string extencion = FileName.Substring(FileName.LastIndexOf('.'));

                int folderNameLength = new DirectoryInfo(DownloadFolder).FullName.Length;
                int extencionLength = extencion.Length;

                FileName = FileName.Substring(0, 250 - (folderNameLength + extencionLength)).Trim();

                FullPath = Path.Combine(new DirectoryInfo(DownloadFolder).FullName, (FileName + extencion));
            }

            return FullPath;
        }

        private void RemoveInvalidCharacters() //TODO: Extract in a library class
        {
            FileName = Regex.Replace(FileName, @"\s{2,}", " ");

            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            FileName = Regex.Replace(FileName, invalidRegStr, "");

            invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()));
            invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            DownloadFolder = Regex.Replace(DownloadFolder, invalidRegStr, "");

        }
        #endregion
    }
}
