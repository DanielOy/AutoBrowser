using HtmlAgilityPack;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AutoBrowser.Core.Browsers
{
    public class WView : BaseBrowser
    {
        private readonly WebView2 _view;
        private HtmlDocument doc;
        private bool documentLoaded;
        private bool cookiesLoaded;
        private List<CoreWebView2Cookie> cookies;

        public WView(WebView2 view)
        {
            _view = view;
            _view.NavigationStarting += _view_NavigationStarting;
        }

        private void _view_NavigationStarting(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs e)
        {
            if (IsAdLink(new Uri(e.Uri)))
            {
                e.Cancel = true;
            }
            else if (!e.Uri.StartsWith("https://")) //TODO: Decide if remove or not the validation
            {
                e.Cancel = true;
            }
            else if (e.Uri.EndsWith(".exe"))
            {
                e.Cancel = true;
            }
            else if (string.IsNullOrEmpty(new Uri(e.Uri).Host))
            {
                e.Cancel = true;
            }
            else
            {
                Library.File.WriteOnFile($"{new Uri(e.Uri).Host}|{e.Uri}", "Pages.dat");
            }
        }

        public override Uri Url => string.IsNullOrEmpty(_view.CoreWebView2.Source) ? null : new Uri(_view.CoreWebView2.Source);

        public override string Cookies => GetCookies();

        private string GetCookies()
        {
            GetCookiesAsync();

            if (!cookiesLoaded)
            {
                cookies = null;

                int timeOut = 0;
                while (cookies == null && timeOut < 30)
                {
                    timeOut++;
                    Wait(1);
                }
                cookiesLoaded = true;
            }

            if (cookies == null)
            {
                return "";
            }
            else
            {
                StringBuilder builder = new StringBuilder();
                cookies.ForEach(x => builder.Append($"{x.Name}={x.Value};"));
                return builder.ToString();
            }
        }

        private async Task<List<CoreWebView2Cookie>> GetCookiesAsync()
        {
            cookies = await _view.CoreWebView2.CookieManager.GetCookiesAsync(Url.AbsoluteUri);
            return cookies;
        }

        public override HtmlDocument Document => GetHtmlDocument();

        private HtmlDocument GetHtmlDocument()
        {
            if (documentLoaded && doc != null)
            {
                return doc;
            }
            else
            {
                doc = null;
                GetHtmlDocumentAsync();

                int timeOut = 0;
                while (doc == null && timeOut < 30)
                {
                    timeOut++;
                    Wait(1);
                }
                documentLoaded = true;

                return doc;
            }
        }

        private async System.Threading.Tasks.Task<HtmlDocument> GetHtmlDocumentAsync()
        {
            string html = await _view.ExecuteScriptAsync("document.documentElement.innerHTML");
            html = System.Text.RegularExpressions.Regex.Unescape(html);
            html = html.Remove(0, 1);
            html = html.Remove(html.Length - 1, 1);

            doc = new HtmlDocument();
            doc.LoadHtml(html);

            return doc;
        }


        public override void Navigate(string url, int timeOut)
        {
            bool isLoading = true;

            _view.NavigationCompleted += (s, e) =>
            {
                isLoading = false;
            };

            if (_view.Source == null)
            {
                _view.Source = new Uri(url);
            }
            else
            {
                _view.CoreWebView2.Navigate(url);
                _view.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                _view.CoreWebView2.Settings.IsScriptEnabled = false;
            }

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

            _view.NavigationCompleted -= null;
            documentLoaded = false;
            cookiesLoaded = false;

            Wait(1);
        }

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
