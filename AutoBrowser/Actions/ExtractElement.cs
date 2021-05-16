using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
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

            HtmlElement element = null;
            foreach (var node in NodePath)
            {
                element = element == null ? GetElement(node, browser) : GetElement(node, element);
            }

            return element;
        }

        private HtmlElement GetElement(Node node, WebBrowser browser)
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
                    throw new Exception("Method not implemented");
                }
                else if (multi.From == MultiNode.Attribute.Tag)
                {
                    int i = Convert.ToInt32(multi.Index);
                    return browser.Document.GetElementsByTagName(node.Value)[i];
                }
            }
            return null;
        }

        private HtmlElement GetElement(Node node, HtmlElement element)
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
                    throw new Exception("Method not implemented");
                }
                else if (multi.From == MultiNode.Attribute.Tag)
                {
                    int i = Convert.ToInt32(multi.Index);
                    return element.GetElementsByTagName(node.Value)[i];
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
        private readonly string _originalValue;
        private readonly object _originalIndex;

        public Attribute From { get; set; }
        public object Index { get; set; }
        public MultiNode(Attribute from, string value, object index)
        {
            From = from;
            Value = _originalValue = value;
            Index = _originalIndex = index;
        }

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
        }

        public override void ResetValues()
        {
            Value = _originalValue;
            Index = _originalIndex;
        }
    }
}
