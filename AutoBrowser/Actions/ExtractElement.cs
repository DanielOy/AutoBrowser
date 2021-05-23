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
        private readonly string _originalName;

        public override Action Action => Action.ExtractElement;
        public List<Node> NodePath { get; set; }
        public string Name { get; set; }

        public ExtractElement(string name, List<Node> nodePath)
        {
            Name = _originalName = name;
            NodePath = nodePath;
        }

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
                if (single.From == SingleNode.Attribute.Id)
                {
                    return browser.Document.GetElementById(node.Value);
                }
                else if (single.From == SingleNode.Attribute.Index)
                {
                    return browser.Document.Body.Children[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode multi)
            {
                if (multi.From == MultiNode.Attribute.Class)
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
                else if (multi.From == MultiNode.Attribute.Tag)
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
                if (single.From == SingleNode.Attribute.Id)
                {
                    foreach (HtmlElement child in element.Children)
                    {
                        if (child.Id == node.Value)
                        {
                            return child;
                        }
                    }
                }
                else if (single.From == SingleNode.Attribute.Index)
                {
                    return element.Children[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode multi)
            {
                if (multi.From == MultiNode.Attribute.Class)
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
                else if (multi.From == MultiNode.Attribute.Tag)
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

                NodePath.ForEach(n => n.ReplaceVariables(item));
            }
        }

        protected override void ResetValues()
        {
            Name = _originalName;
            NodePath.ForEach(n => n.ResetValues());
        }
    }

    public abstract class Node
    {
        //public int Order { get; set; }
        //public abstract object From { get; protected set; }
        public string Value { get; set; }

        //public enum Attribute { }
        public abstract void ReplaceVariables(KeyValuePair<string, object> savedValues);
        public abstract void ResetValues();

    }

    public class SingleNode : Node
    {
        private readonly string _originalValue;

        public Attribute From { get; set; }

        public enum Attribute
        {
            Id = 0,
            Index = 1
        }

        public SingleNode(Attribute from, string value)
        {
            From = from;
            Value = _originalValue = value;
        }

        public override void ReplaceVariables(KeyValuePair<string, object> variable)
        {
            if (Value.Contains($"[{variable.Key}]"))
            {
                Value = Value.Replace($"[{variable.Key}]", variable.Value.ToString());
            }
        }

        public override void ResetValues()
        {
            Value = _originalValue;
        }
    }

    public class MultiNode : Node
    {
        #region Variables
        private readonly string _originalValue;
        private readonly object _originalIndex;
        private readonly object _originalClassName;
        #endregion

        #region Properties
        public Attribute From { get; set; }
        public object Index { get; set; }
        public object ClassName { get; set; }
        #endregion


        #region Constructors
        public MultiNode(HtmlTag tag)
        {
            From = Attribute.Tag;
            Value = _originalValue = tag.Value;
            Index = _originalIndex = "";
            ClassName = _originalClassName = "";
        }

        public MultiNode(HtmlTag tag, object index)
        {
            From = Attribute.Tag;
            Value = _originalValue = tag.Value;
            Index = _originalIndex = index;
            ClassName = _originalClassName = "";
        }

        public MultiNode(HtmlTag tag, object className, object index)
        {
            From = Attribute.Class;
            Value = _originalValue = tag.Value;
            ClassName = _originalClassName = className;
            Index = _originalIndex = index;
        }
        #endregion

        public enum Attribute
        {
            Tag = 2,
            Class = 3
        }

        public override void ReplaceVariables(KeyValuePair<string, object> variable)
        {
            if (Value.Contains($"[{variable.Key}]"))
            {
                Value = Value.Replace($"[{variable.Key}]", variable.Value.ToString());
            }

            if (Index.ToString().Contains($"[{variable.Key}]"))
            {
                Index = Index.ToString().Replace($"[{variable.Key}]", variable.Value.ToString());
            }

            if (ClassName.ToString().Contains($"[{variable.Key}]"))
            {
                ClassName = ClassName.ToString().Replace($"[{variable.Key}]", variable.Value.ToString());
            }
        }

        public override void ResetValues()
        {
            Value = _originalValue;
            Index = _originalIndex;
            ClassName = _originalClassName;
        }
    }
}
