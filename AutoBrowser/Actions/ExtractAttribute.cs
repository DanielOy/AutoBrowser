using System;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class ExtractAttribute : BaseAction
    {
        public override WebAction Action => WebAction.ExtractAtribute;
        public string Variable { get; set; }
        public string AttributeName { get; set; }
        public string Name { get; set; }

        public ExtractAttribute(string variable, string attribute, string name)
        {
            Variable = variable;
            AttributeName = attribute;
            Name = name;
        }

        public override object Perform(WebBrowser browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException(nameof(browser));
            }
            if (string.IsNullOrEmpty(Variable))
            {
                throw new ArgumentNullException(nameof(Variable));
            }
            if (string.IsNullOrEmpty(AttributeName))
            {
                throw new ArgumentNullException(nameof(AttributeName));
            }

            return browser.Document.Body.GetAttribute(AttributeName);
        }
        public object Perform(HtmlElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }
            if (string.IsNullOrEmpty(AttributeName))
            {
                throw new ArgumentNullException(nameof(AttributeName));
            }

            return element.GetAttribute(AttributeName);
        }
    }
}
