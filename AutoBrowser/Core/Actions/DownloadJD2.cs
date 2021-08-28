using AutoBrowser.Core.Browsers;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AutoBrowser.Core.Actions
{
    public class DownloadJD2 : BaseDownload
    {
        #region Properties
        public bool StartQueue { get; set; } = true;
        #endregion

        #region Constructor
        public DownloadJD2() { }
        public DownloadJD2(string url, string fileName) : base(url, fileName)
        {
            Url = _originalUrl = url;
            FileName = _originalfileName = fileName;
            DownloadFolder = _originalDownloadFolder = DownloadFolder;
        }

        public DownloadJD2(string url, string fileName, string downloadFolder) : base(url, fileName, downloadFolder)
        {
            Url = _originalUrl = url;
            FileName = _originalfileName = fileName;
            DownloadFolder = _originalDownloadFolder = downloadFolder;
        }
        #endregion

        #region Functions
        public override object Perform(BaseBrowser browser)
        {

            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentNullException(nameof(Url));
            }

            if (!string.IsNullOrEmpty(FileName))
            {
                FileInfo fileToDownload = GetValidFileInfo();

                if (fileToDownload.Exists)
                {
                    return true;
                }

                GenerateDownloadFile(Url, fileToDownload.Name, fileToDownload.DirectoryName);
            }
            else
            {
                if (!Directory.Exists(DownloadFolder))
                {
                    Directory.CreateDirectory(DownloadFolder);
                }

                string FolderPath = SharedLibrary.File.FormatValidFileName(DownloadFolder);

                GenerateDownloadFile(Url, "", FolderPath);
            }

            InitJdownloader2();

            return true;
        }

        private void GenerateDownloadFile(string url, string fileName, string downloadFolder)
        {
            StringBuilder content = new StringBuilder();
            content.AppendLine($"text={url}");
            content.AppendLine($"downloadFolder = {downloadFolder}");

            if (StartQueue)
            {
                content.AppendLine("autoConfirm = TRUE");
                content.AppendLine("autoStart = TRUE");
                content.AppendLine("forcedStart = TRUE");
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                content.AppendLine($"filename = {fileName}");
            }

            SharedLibrary.File.WriteOnFile(content.ToString(), Path.Combine(Global.JDownloaderFilesPath, $"AutoWeb{DateTime.Now.ToString("MMdd_hh_mm_ss")}.crawljob"));
        }

        private void InitJdownloader2()
        {
            var _Process = Process.GetProcessesByName("JDownloader2");
            if (_Process == null || _Process.Length == 0)
            {
                Process.Start(Global.JDownloaderExePath);
            }
        }

        internal override void InitVariables()
        {
            _originalUrl = Url;
            _originalfileName = FileName;
            _originalDownloadFolder = DownloadFolder;
        }

        public override string GetDescription()
        {
            return $"JDownload <{Url}> in <{FileName}>";
        }
        #endregion
    }
}
