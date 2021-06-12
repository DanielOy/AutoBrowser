using System.Collections.Generic;
using System.IO;
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

            string FullPath = Path.Combine(new DirectoryInfo(DownloadFolder).FullName, FileName);

            FullPath = Library.File.FormatValidFileName(FullPath);

            return new FileInfo(FullPath);
        }

        #endregion
    }
}
