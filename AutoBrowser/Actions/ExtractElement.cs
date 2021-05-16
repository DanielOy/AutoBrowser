using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class ExtractElement : BaseAction
    {
        public override WebAction Action => WebAction.ExtractElement;
        public List<Node> NodePath { get; set; }
        public string Name { get; set; }

        public ExtractElement(string name, List<Node> nodePath)
        {
            Name = name;
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
            if (node is SingleNode)
            {
                if ((node as SingleNode).From == SingleNode.Attribute.Id)
                {
                    return browser.Document.GetElementById(node.Value);
                }
                else if ((node as SingleNode).From == SingleNode.Attribute.Index)
                {
                    return browser.Document.Body.Children[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode)
            {
                if ((node as MultiNode).From == MultiNode.Attribute.Class)
                {
                    throw new Exception("Method not implemented");
                }
                else if ((node as MultiNode).From == MultiNode.Attribute.Tag)
                {
                    return browser.Document.GetElementsByTagName(node.Value)[(node as MultiNode).Index];
                }
            }
            return null;
        }

        private HtmlElement GetElement(Node node, HtmlElement element)
        {
            if (node is SingleNode)
            {
                if ((node as SingleNode).From == SingleNode.Attribute.Id)
                {
                    foreach (HtmlElement child in element.Children)
                    {
                        if (child.Id == node.Value)
                        {
                            return child;
                        }
                    }
                }
                else if ((node as SingleNode).From == SingleNode.Attribute.Index)
                {
                    return element.Children[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode)
            {
                if ((node as MultiNode).From == MultiNode.Attribute.Class)
                {
                    throw new Exception("Method not implemented");
                }
                else if ((node as MultiNode).From == MultiNode.Attribute.Tag)
                {
                    return element.GetElementsByTagName(node.Value)[(node as MultiNode).Index];
                }
            }
            return null;
        }
    }

    public abstract class Node
    {
        //public int Order { get; set; }
        //public abstract object From { get; protected set; }
        public string Value { get; set; }

        //public enum Attribute { }

    }

    public class SingleNode : Node
    {
        public Attribute From { get; set; }

        public enum Attribute
        {
            Id = 0,
            Index = 1
        }

        public SingleNode(Attribute from, string value)
        {
            From = from;
            Value = value;
        }
    }

    public class MultiNode : Node
    {
        public Attribute From { get; set; }
        public int Index { get; set; }
        public MultiNode(Attribute from, string value, int index)
        {
            From = from;
            Value = value;
            Index = index;
        }

        public enum Attribute
        {
            Tag = 2,
            Class = 3
        }
    }
}
