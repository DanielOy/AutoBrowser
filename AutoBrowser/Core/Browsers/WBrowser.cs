using System;
using System.Windows.Forms;

namespace AutoBrowser.Core.Browsers
{
    public class WBrowser : BaseBrowser
    {
        #region Global Variables
        private WebBrowser _browser;
        #endregion

        #region Properties
        public override Uri Url => _browser?.Url;

        public override string Cookies =>SharedLibrary.Web.GetCookie(Url?.AbsoluteUri);

        public override HtmlAgilityPack.HtmlDocument Document => GetDocument();

        private HtmlAgilityPack.HtmlDocument GetDocument()
        {
            string html = _browser.Document.Body.OuterHtml;
            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
        #endregion

        #region Constructors
        public WBrowser() : base()
        {
            _browser = new WebBrowser() { ScriptErrorsSuppressed = true };
            _browser.Navigating += _browser_Navigating;
            _browser.NewWindow += _browser_NewWindow;
        }

        public WBrowser(WebBrowser browser) : base()
        {
            _browser = browser;
            _browser.ScriptErrorsSuppressed = true;
            _browser.Navigating += _browser_Navigating;
            _browser.NewWindow += _browser_NewWindow;
        }
        #endregion

        #region Functions
        public override void Navigate(string url, int timeOut)
        {
            bool isLoading = true;

            _browser.DocumentCompleted += (s, e) =>
            {
                isLoading = false;
            };

            _browser.Navigate(url);

            if (timeOut > 0)
            {
                int _waitedTime = 0;
                while (isLoading && _waitedTime < timeOut)
                {
                    Wait(1);
                    _waitedTime++;
                }
            }
            else
            {
                while (isLoading)
                {
                    Wait(1);
                }
            }

            _browser.DocumentCompleted -= null;
            Wait(1);
        }

        #endregion

        #region Events
        private void _browser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void _browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (IsAdLink(e.Url))
            {
                e.Cancel = true;
            }
            else if (e.Url.AbsoluteUri.EndsWith(".exe"))
            {
                e.Cancel = true;
            }
            else if (string.IsNullOrEmpty(e.Url.Host))
            {
                e.Cancel = true;
            }
            else
            {
               SharedLibrary.File.WriteOnFile($"{e.Url.Host}|{e.Url.AbsoluteUri}", "Pages.dat");
            }
        }

        #endregion
    }
}
