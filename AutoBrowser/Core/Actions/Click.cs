using AutoBrowser.Core.Browsers;
using System;
using System.Windows.Forms;

namespace AutoBrowser.Core.Actions
{
    public class Click : WebAction
    {
        #region Global Variables
        private string _originalVariable;
        #endregion

        #region Properties
        public string Variable { get; set; }
        #endregion

        #region Constructors
        public Click()
        {
        }

        public Click(string variable)
        {
            Variable = _originalVariable = variable;
        }
        #endregion

        #region Functions
        public override object Perform(BaseBrowser browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException(nameof(browser));
            }

            browser.Document.Body.InvokeMember("Click");
            return true;
        }

        public object Perform(HtmlElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.InvokeMember("Click");
            return true;
        }

        protected override void ResetValues()
        {
            Variable = _originalVariable;
        }

        internal override void InitVariables()
        {
            _originalVariable = Variable;
        }

        public override string GetDescription()
        {
            return $"Click <{Variable}>";
        }
        #endregion
    }
}
