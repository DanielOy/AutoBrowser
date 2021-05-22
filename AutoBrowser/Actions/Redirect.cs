using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class Redirect : WebAction
    {
        private readonly string _originalUrl;

        public override Action Action => Action.Redirect;

        public string Url { get; set; }
        public int TimeOut { get; set; }

        public Redirect(string url)
        {
            Url = _originalUrl = url;
        }

        public Redirect(string url, int timeOut)
        {
            Url = _originalUrl = url;
            TimeOut = timeOut;
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

            bool isLoading = true;

            browser.DocumentCompleted += (s, e) =>
            {
                isLoading = false;
            };

            browser.Navigate(Url);

            if (TimeOut > 0)
            {
                int _waitedTime = 0;
                while (isLoading && _waitedTime < TimeOut)
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


            browser.DocumentCompleted -= null;
            Wait(3); //UNDO: Remove after, create an action wait
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
                    Url = Url.Replace($"[{item.Key}]", item.Value.ToString());
                }
            }
        }

        protected override void ResetValues()
        {
            Url = _originalUrl;
        }
    }
}
