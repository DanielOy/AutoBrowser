using System;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class Redirect : WebAction
    {
        #region Global Variables
        private string _originalUrl;
        #endregion

        #region Properties

        public string Url { get; set; }

        public int TimeOut { get; set; }
        #endregion

        #region Constructors
        public Redirect()
        {
        }

        public Redirect(string url)
        {
            Url = _originalUrl = url;
        }

        public Redirect(string url, int timeOut)
        {
            Url = _originalUrl = url;
            TimeOut = timeOut;
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

        protected override void ResetValues()
        {
            Url = _originalUrl;
        }

        internal override void InitVariables()
        {
            _originalUrl = Url;
        } 
        #endregion
    }
}
