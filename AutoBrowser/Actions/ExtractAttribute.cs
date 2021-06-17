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
        #region Global Variables
        private string _originalVariable;
        private string _originalAttributeName;
        private string _originalName;
        #endregion

        #region Properties
        public string Variable { get; set; }
        public string AttributeName { get; set; }
        public string Name { get; set; }
        #endregion

        #region Constructor
        public ExtractAttribute() { }
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
            if (AttributeName=="text") //FIX: requiere to be standard
            {
                return element.InnerText;
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

        protected override void ResetValues()
        {
            Variable = _originalVariable;
            AttributeName = _originalAttributeName;
            Name = _originalName;
        }

        internal override void InitVariables()
        {
            _originalVariable = Variable;
            _originalAttributeName = AttributeName;
            _originalName = Name;
        }
        #endregion
    }
}
