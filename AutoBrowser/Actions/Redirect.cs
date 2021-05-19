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

        public Redirect(string url)
        {
            Url = _originalUrl = url;
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
                string x = e.Url.AbsoluteUri; //UNDONE: Remove, only for testing
                isLoading = false;
            };

            browser.Navigate(Url);

            while (isLoading)
            {
                Wait(1);
            }

            browser.DocumentCompleted -= null;
            Wait(3); //UNDO: Remove after
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
