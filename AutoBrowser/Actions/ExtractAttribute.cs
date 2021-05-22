using AutoBrowser.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    [DebuggerDisplay("{Name} = {Variable} => {AttributeName}")]
    public class ExtractAttribute : WebAction
    {
        #region Variables
        private readonly string _originalVariable;
        private readonly string _originalAttributeName;
        private readonly string _originalName;
        #endregion

        #region Properties
        public override Action Action => Action.ExtractAtribute;
        public string Variable { get; set; }
        public string AttributeName { get; set; }
        public string Name { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Get the value of an attribute of the element with the variable name.
        /// </summary>
        /// <param name="name">Name with the attribute value will be saved.</param>
        /// <param name="variable">Name of a element, this need to be previously extracted</param>
        /// <param name="attribute">HTMLAtribute that represent the attribute that will be extracted.</param>
        public ExtractAttribute(string name, string variable, HtmlAttribute attribute)
        {
            Variable = _originalVariable = variable;
            AttributeName = _originalAttributeName = attribute.Value;
            Name = _originalName = name;
        }

        /// <summary>
        /// Get the value of an attribute of the element with the variable name.
        /// </summary>
        /// <param name="name">Name with the attribute value will be saved.</param>
        /// <param name="variable">Name of a element, this need to be previously extracted</param>
        /// <param name="attribute">Name of the attribute that will be extracted.</param>
        public ExtractAttribute(string name, string variable, string attribute)
        {
            Variable = _originalVariable = variable;
            AttributeName = _originalAttributeName = attribute;
            Name = _originalName = name;
        }
        #endregion

        #region Functions
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

        public object Perform(List<HtmlElement> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (string.IsNullOrEmpty(AttributeName))
            {
                throw new ArgumentNullException(nameof(AttributeName));
            }

            if (AttributeName != Enums.HtmlAttribute.length.Value)
            {
                throw new Exception("Only 'length' value can be extracted from the element collection.");
            }

            return elements.Count;
        }

        public object Perform(HtmlElementCollection elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            if (string.IsNullOrEmpty(AttributeName))
            {
                throw new ArgumentNullException(nameof(AttributeName));
            }

            if (AttributeName != Enums.HtmlAttribute.length.Value)
            {
                throw new Exception("Only 'length' value can be extracted from the element collection.");
            }

            return elements.Count;
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
        #endregion
    }
}
