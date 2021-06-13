using AutoBrowser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    //TODO: Permit to get a list of elements, don't only one, and make operations with the list.
    //TODO: Extract elements from elements (childs)
    public class ExtractElement : WebAction
    {
        #region Global Variables
        private string _originalName;
        #endregion

        #region Properties
        public List<Node> NodePath { get; set; }
        public string Name { get; set; }
        #endregion

        #region Constructors
        public ExtractElement() { }

        public ExtractElement(string name, List<Node> nodePath)
        {
            Name = _originalName = name;
            NodePath = nodePath;
        } 
        #endregion

        public override object Perform(WebBrowser browser)
        {
            if (browser == null)
            {
                throw new ArgumentNullException(nameof(browser));
            }
            if (NodePath == null)
            {
                throw new ArgumentNullException(nameof(NodePath));
            }

            object element = null;
            foreach (var node in NodePath)
            {
                //FIX: Need to make dynamic the GetElement for element or a list of elements
                element = element == null ? GetElement(node, browser) : GetElement(node, (element as HtmlElement));
            }

            return element;
        }

        private object GetElement(Node node, WebBrowser browser)
        {
            if (node is SingleNode single)
            {
                if (single.From == SingleNode.SingleNodeType.Id)
                {
                    return browser.Document.GetElementById(node.Value);
                }
                else if (single.From == SingleNode.SingleNodeType.Index)
                {
                    return browser.Document.Body.Children[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode multi)
            {
                if (multi.From == MultiNode.MultiNodeType.Class)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        var elements = new List<HtmlElement>(browser.Document.GetElementsByTagName(multi.Value).Cast<HtmlElement>());
                        elements = elements
                            .Where(x => x.GetAttribute(HtmlAttribute.ClassName.Value).Contains(multi.ClassName.ToString()))
                            .ToList();

                        return elements;
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);

                        var elements = new List<HtmlElement>(browser.Document.GetElementsByTagName(multi.Value).Cast<HtmlElement>());
                        elements = elements
                            .Where(x => x.GetAttribute(HtmlAttribute.ClassName.Value).Contains(multi.ClassName.ToString()))
                            .ToList();

                        return elements?[i];
                    }
                }
                else if (multi.From == MultiNode.MultiNodeType.Tag)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        return browser.Document.GetElementsByTagName(node.Value);
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);
                        return browser.Document.GetElementsByTagName(node.Value)?[i];
                    }
                }
            }
            return null;
        }

        private object GetElement(Node node, HtmlElement element)
        {
            if (node is SingleNode single)
            {
                if (single.From == SingleNode.SingleNodeType.Id)
                {
                    foreach (HtmlElement child in element.Children)
                    {
                        if (child.Id == node.Value)
                        {
                            return child;
                        }
                    }
                }
                else if (single.From == SingleNode.SingleNodeType.Index)
                {
                    return element.Children[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode multi)
            {
                if (multi.From == MultiNode.MultiNodeType.Class)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        var elements = new List<HtmlElement>(element.GetElementsByTagName(multi.Value).Cast<HtmlElement>());
                        elements = elements
                            .Where(x => x.GetAttribute(HtmlAttribute.ClassName.Value).Equals(multi.ClassName))
                            .ToList();
                        var auxElement = (new WebBrowser()).Document.CreateElement("div");

                        elements.ForEach(x => auxElement.AppendChild(x));
                        return auxElement.Children;
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);

                        var elements = new List<HtmlElement>(element.GetElementsByTagName(multi.Value).Cast<HtmlElement>());
                        elements = elements
                            .Where(x => x.GetAttribute(HtmlAttribute.ClassName.Value).Equals(multi.ClassName))
                            .ToList();

                        return elements[i];
                    }
                }
                else if (multi.From == MultiNode.MultiNodeType.Tag)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        return element.GetElementsByTagName(node.Value);
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);
                        return element.GetElementsByTagName(node.Value)[i];
                    }
                }
            }
            return null;
        }

        public new void ReplaceVariables(Dictionary<string, object> savedValues)
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

                NodePath.ForEach(n => n.ReplaceVariables(item));
            }
        }

        protected override void ResetValues()
        {
            Name = _originalName;
            NodePath.ForEach(n => n.ResetValues());
        }

        internal override void InitVariables()
        {
            _originalName = Name;
            NodePath.ForEach(n => n.InitVariables());
        }
    }

}
