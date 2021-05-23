using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class Download : BaseDownload
    {
        #region Properties
        public override Action Action => Action.Download;

        public bool ReplaceFile { get; set; }
        #endregion

        #region Constructor
        public Download(string url, string fileName) : base(url, fileName)
        {
            Url = _originalUrl = url;
            FileName = _originalfileName = fileName;
            DownloadFolder = _originalDownloadFolder = DownloadFolder;
        }

        public Download(string url, string fileName, string downloadFolder) : base(url, fileName, downloadFolder)
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

                WriteFile.WriteOnFile($"[{DateTime.Now.ToString("dd/MM hh:mm:ss")}] {filePath.Substring(filePath.LastIndexOf("\\") + 1)}", "DownloadHistory");
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
