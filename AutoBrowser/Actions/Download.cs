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
        public bool ReplaceFile { get; set; }
        public int TimeOut { get; set; }
        public event ProgressChangedEventHandler ProgressChanged;
        #endregion

        #region Constructor
        public Download() { }
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
            DateTime lastUpdate = DateTime.Now;
            int lastPorcent = 0;
            wc.DownloadFileCompleted += (s, e) => { downloadFinished = true; };
            wc.DownloadProgressChanged += (s, e) =>
            {
                string sizeDownloaded = Library.TextFormat.NoBytesToSize(e.BytesReceived);
                string sizeTotal = Library.TextFormat.NoBytesToSize(e.TotalBytesToReceive);

                if ((DateTime.Now - lastUpdate).Seconds >= 1 && lastPorcent < e.ProgressPercentage)
                {
                    lastUpdate = DateTime.Now;
                    lastPorcent = e.ProgressPercentage;

                    ProgressChanged?.Invoke(this, new Classes.ProgressChangedArgs(
                        $"Downloading [{e.ProgressPercentage}%] " +
                        $"[{sizeDownloaded}/{sizeTotal}] " +
                        $"{downloadFile.Name}"));
                }
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

        internal override void InitVariables()
        {
            _originalUrl = Url;
            _originalfileName = FileName;
            _originalDownloadFolder = DownloadFolder;
        }
        #endregion
    }
}
