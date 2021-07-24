using AutoBrowser.Core.Browsers;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AutoBrowser.Core.Actions
{
    public class DownloadIDM : BaseDownload
    {
        #region Properties
        public bool StartQueue { get; set; } = true;
        #endregion

        #region Constructors
        public DownloadIDM() { }
        public DownloadIDM(string url, string fileName) : base(url, fileName)
        {
            Url = _originalUrl = url;
            FileName = _originalfileName = fileName;
            DownloadFolder = _originalDownloadFolder = DownloadFolder;
        }

        public DownloadIDM(string url, string fileName, string downloadFolder) : base(url, fileName, downloadFolder)
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

            FileInfo fileToDownload = GetValidFileInfo();

            if (fileToDownload.Exists)
            {
                return true;
            }

            LauchDownload(fileToDownload);

            return true;
        }

        internal override void InitVariables()
        {
            _originalUrl = Url;
            _originalfileName = FileName;
            _originalDownloadFolder = DownloadFolder;
        }

        /*
         /d [URL] - downloads a file
         /s - starts queue in scheduler
         /p [local_path] - defines the local path where to save the file
         /f [local_file_name] - defines the local file name to save the file
         /q - IDM will exit after the successful downloading. This parameter works only for the first copy
         /h - IDM will hang up your connection after the successful downloading
         /n - turns on the silent mode when IDM doesn't ask any questions
         /a - add a file specified with /d to download queue, but don't start downloading
         */

        private void LauchDownload(FileInfo downloadInfo)
        {
            Process idmProcess = new Process
            {
                StartInfo = new ProcessStartInfo(Global.IDMExePath)
                {
                    Arguments = $"/q /n " +
                    $"{(StartQueue ? "/s " : "/a ")} " +
                    $"/d \"{Url}\" /p \"{downloadInfo.DirectoryName}\" " +
                    $"{(string.IsNullOrEmpty(downloadInfo.Name) ? "" : $"/f \"{downloadInfo.Name}\"")}",
                    UseShellExecute = false
                }
            };
            idmProcess.Start();
        }

        public override string GetDescription()
        {
            return $"IDownload <{Url}> in <{FileName}>";
        }
        #endregion
    }
}
