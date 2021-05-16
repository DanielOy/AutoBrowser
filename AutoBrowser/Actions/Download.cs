using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class Download : BaseAction
    {
        public override WebAction Action => WebAction.Download;

        public string DownloadFolder { get; set; } = Path.Combine(Application.StartupPath, "Downloads");
        public string FileName { get; set; }
        public string Url { get; set; }

        public Download(string url, string fileName)
        {
            Url = url;
            FileName = fileName;
        }

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

            foreach (var item in savedValues)
            {
                Url = Url.Replace($"[{item.Key}]", item.Value.ToString());
                DownloadFolder = DownloadFolder.Replace($"[{item.Key}]", item.Value.ToString());
                FileName = FileName.Replace($"[{item.Key}]", item.Value.ToString());
            }
        }
    }
}
