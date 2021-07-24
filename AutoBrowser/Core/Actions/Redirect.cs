using AutoBrowser.Core.Browsers;
using System;

namespace AutoBrowser.Core.Actions
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
        public override object Perform(BaseBrowser browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException(nameof(browser));
            }
            if (string.IsNullOrEmpty(Url))
            {
                throw new ArgumentNullException(nameof(Url));
            }

            browser.Navigate(Url, TimeOut);

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

        public override string GetDescription()
        {
            return $"Go to <{Url}>";
        }
        #endregion
    }
}
