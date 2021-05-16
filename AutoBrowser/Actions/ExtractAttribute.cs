using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class ExtractAttribute : WebAction
    {
        private readonly string _originalVariable;
        private readonly string _originalAttributeName;
        private readonly string _originalName;
        #region Properties
        public override Action Action => Action.ExtractAtribute;
        public string Variable { get; set; }
        public string AttributeName { get; set; }
        public string Name { get; set; }
        #endregion

        #region Constructor
        public ExtractAttribute(string variable, string attribute, string name)
        {
            Variable = _originalVariable = variable;
            AttributeName = _originalAttributeName = attribute;
            Name = _originalName = name;
        }
        #endregion

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

        public override void ReplaceVariables(Dictionary<string, object> savedValues)
        {
            if (savedValues == null)
            {
                return;
            }

            ResetValues();

            foreach (var item in savedValues)
            {
                if (Name.Contains($"[{item.Key}]"))
                {
                    Name = Name.Replace($"[{item.Key}]", item.Value.ToString());
                }

                if (AttributeName.Contains($"[{item.Key}]"))
                {
                    AttributeName = AttributeName.Replace($"[{item.Key}]", item.Value.ToString());
                }

                if (Variable.Contains($"[{item.Key}]"))
                {
                    Variable = Variable.Replace($"[{item.Key}]", item.Value.ToString());
                }
            }
        }

        protected override void ResetValues()
        {
            Variable = _originalVariable;
            AttributeName = _originalAttributeName;
            Name = _originalName;
        }
    }
}
