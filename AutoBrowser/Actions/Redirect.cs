using System;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class Redirect : BaseAction
    {
        public override WebAction Action => WebAction.Redirect;

        public string Url { get; set; }

        public Redirect(string url)
        {
            Url = url;
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
                string x = e.Url.AbsoluteUri; //FIX: Remove, only for testing
                isLoading = false;
            };

            browser.Navigate(Url);

            while (isLoading)
            {
                Wait(1);
            }

            browser.DocumentCompleted -= null;

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
    }
}
