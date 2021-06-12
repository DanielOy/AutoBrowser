using System;
using System.IO;
using System.Net;
using System.Windows.Forms;
using static AutoBrowser.Classes.AutoWebBrowser;

namespace AutoBrowser.Actions
{
    public class Download : BaseDownload
    {
        #region Properties
        public override Action Action => Action.Download;

        public bool ReplaceFile { get; set; }
        public int TimeOut { get; set; }
        public event ProgressChangedEventHandler ProgressChanged;
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

            FileInfo downloadFile = GetValidFileInfo();

            if (!ReplaceFile && downloadFile.Exists)
            {
                return true;
            }

            WebClient wc = new WebClient();
            wc.Headers.Add(HttpRequestHeader.Cookie, Library.Web.GetCookie(browser?.Url?.AbsoluteUri));
            wc.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:88.0) Gecko/20100101 Firefox/88.0");

            bool downloadFinished = false;
            wc.DownloadFileCompleted += (s, e) => { downloadFinished = true; };
            wc.DownloadProgressChanged += (s, e) =>
            {
                string sizeDownloaded = Library.TextFormat.NoBytesToSize(e.BytesReceived);
                string sizeTotal = Library.TextFormat.NoBytesToSize(e.TotalBytesToReceive);

                ProgressChanged?.Invoke(this, new Classes.ProgressChangedArgs(
                    $"Downloading [{e.ProgressPercentage}%] " +
                    $"[{sizeDownloaded}/{sizeTotal}] " +
                    $"{downloadFile.Name}"));
            };
            wc.DownloadFileAsync(new Uri(Url), downloadFile.FullName);

            if (TimeOut > 0)
            {
                int _waitedTime = 0;
                while (!downloadFinished && _waitedTime < TimeOut)
                {
                    Wait(1);
                    _waitedTime++;
                }
            }
            else
            {
                while (!downloadFinished)
                {
                    Wait(1);
                }
            }

            Library.File.WriteOnFile($"[{DateTime.Now.ToString("dd/MM hh:mm:ss")}] {downloadFile.Name}", "DownloadHistory");

            return true;
        }

        private void Wait(int seconds)
        {
            DateTime finalTime = DateTime.Now.AddSeconds(seconds);

            while (finalTime > DateTime.Now)
            {
                Application.DoEvents();
            }
        }
        #endregion
    }
}
