using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class Download : WebAction
    {
        #region Properties
        public override Action Action => Action.Download;
        public string DownloadFolder { get; set; } = Path.Combine(Application.StartupPath, "Downloads");
        public string FileName { get; set; }
        public string Url { get; set; }
        public bool ReplaceFile { get; set; }
        #endregion

        #region Variables
        private readonly string _originalfileName;
        private readonly string _originalDownloadFolder;
        private readonly string _originalUrl;

        #endregion

        #region Constructor
        public Download(string url, string fileName)
        {
            Url = _originalUrl = url;
            FileName = _originalfileName = fileName;
            DownloadFolder = _originalDownloadFolder = DownloadFolder;
        }

        public Download(string url, string fileName, string downloadFolder)
        {
            Url = _originalUrl = url;
            FileName = _originalfileName = fileName;
            DownloadFolder = _originalDownloadFolder = downloadFolder;
        }
        #endregion

        #region Functions
        public override object Perform(WebBrowser browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException(nameof(browser));
            }

            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentNullException(nameof(Url));
            }

            string filePath = GetFileFullPath();

            if (!ReplaceFile && File.Exists(filePath))
            {
                return true;
            }

            WebClient wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.Cookie, Classes.WebTools.GetCookie(browser?.Url?.AbsoluteUri));
            wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");
            try
            {
                wc.DownloadFile(Url, filePath);

                WriteFile.WriteOnFile("DownloadHistory", $"[{DateTime.Now.ToString("dd/MM hh:mm:ss")}] {filePath.Substring(filePath.LastIndexOf("\\") + 1)}");
            }
            catch
            {
                return false;
            }
            return true;
        }

        private string GetFileFullPath()
        {
            if (!Directory.Exists(DownloadFolder))
            {
                Directory.CreateDirectory(DownloadFolder);
            }

            RemoveInvalidCharacters();

            string FullPath = Path.Combine(new DirectoryInfo(DownloadFolder).FullName, FileName);

            FullPath = FormatValidLength(FullPath);

            return FullPath;
        }

        private string FormatValidLength(string FullPath)
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

        private void RemoveInvalidCharacters()
        {
            FileName = Regex.Replace(FileName, @"\s{2,}", " ");

            string invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            FileName = Regex.Replace(FileName, invalidRegStr, "");

            invalidChars = Regex.Escape(new string(Path.GetInvalidPathChars()));
            invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            DownloadFolder = Regex.Replace(DownloadFolder, invalidRegStr, "");

        }

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
        #endregion
    }
}
