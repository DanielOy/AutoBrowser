using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

            WebClient wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.Cookie, Classes.WebTools.GetCookie(browser.Url.AbsoluteUri));
            wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");

            if (!Directory.Exists(DownloadFolder))
            {
                Directory.CreateDirectory(DownloadFolder);
            }

            wc.DownloadFile(Url, Path.Combine(DownloadFolder, FileName));
            return true;
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
                    Url = _originalUrl.Replace($"[{item.Key}]", item.Value.ToString());
                }

                if (DownloadFolder.Contains($"[{item.Key}]"))
                {
                    DownloadFolder = _originalDownloadFolder.Replace($"[{item.Key}]", item.Value.ToString());
                }

                if (FileName.Contains($"[{item.Key}]"))
                {
                    FileName = _originalfileName.Replace($"[{item.Key}]", item.Value.ToString());
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
