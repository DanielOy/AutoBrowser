using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class Click : WebAction
    {
        private readonly string _originalVariable;

        public override Action Action => Action.Click;
        public string Variable { get; set; }

        public Click(string variable)
        {
            Variable = _originalVariable = variable;
        }

        public override object Perform(WebBrowser browser)
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
    }
}
