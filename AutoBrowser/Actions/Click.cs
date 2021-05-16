using System;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class Click : BaseAction
    {
        public override WebAction Action => WebAction.Click;
        public string Variable { get; set; }

        public Click(string variable)
        {
            Variable = variable;
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
    }
}
