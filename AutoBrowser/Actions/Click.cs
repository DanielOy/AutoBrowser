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

        public override void ReplaceVariables(Dictionary<string, object> savedValues)
        {
            if (savedValues == null)
            {
                return;
            }

            ResetValues();

            foreach (var item in savedValues)
            {
                if (Variable.Contains($"[{item.Key}]"))
                {
                    Variable = Variable.Replace($"[{item.Key}]", item.Value.ToString());
                }
            }
        }

        protected override void ResetValues()
        {
            Variable = _originalVariable;
        }
    }
}
