using AutoBrowser.Core.Browsers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AutoBrowser.Core.Actions
{
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

        public override object Perform(BaseBrowser browser)
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
                switch (element)
                {
                    case List<HtmlNode> list:
                        element = GetElement(node, list);
                        break;
                    case HtmlNode nod:
                        element = GetElement(node, nod);
                        break;
                    default:
                        element = GetElement(node, browser);
                        break;
                }
            }

            return element;
        }

        private object GetElement(Node node, List<HtmlNode> list)
        {
            if (node is SingleNode single)
            {
                if (single.From == SingleNode.SingleNodeType.Id)
                {
                    foreach (HtmlNode child in list)
                    {
                        if (child.Id == node.Value)
                        {
                            return child;
                        }
                    }
                }
                else if (single.From == SingleNode.SingleNodeType.Index)
                {
                    return list[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode multi)
            {
                if (multi.From == MultiNode.MultiNodeType.Class)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        var elements = list.Where(x => x.Name == multi.Value);
                        elements = elements?
                            .Where(x => x.GetAttributeValue(Enums.HtmlAttribute.ClassName.Value, "").Equals(multi.ClassName))?
                            .ToList();

                        return elements;
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);

                        var elements = list.Where(x => x.Name == multi.Value).ToList();
                        elements = elements?
                            .Where(x => x.GetAttributeValue(Enums.HtmlAttribute.ClassName.Value, "").Equals(multi.ClassName))?
                            .ToList();

                        return elements[i];
                    }
                }
                else if (multi.From == MultiNode.MultiNodeType.Tag)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        return list.Where(x => x.Name == node.Value)?.ToList();
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);
                        return list.Where(x => x.Name == node.Value).ToList()[i];
                    }
                }
            }
            return null;
        }

        private object GetElement(Node node, BaseBrowser browser)
        {
            if (node is SingleNode single)
            {
                if (single.From == SingleNode.SingleNodeType.Id)
                {
                    return browser.Document.GetElementbyId(node.Value);
                }
                else if (single.From == SingleNode.SingleNodeType.Index)
                {
                    return browser.Document?.DocumentNode?
                        .Descendants("Body")?.ToArray()?[0]
                        .Descendants()?.ToArray()?[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode multi)
            {
                if (multi.From == MultiNode.MultiNodeType.Class)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        var elements = browser.Document.DocumentNode?.Descendants(multi.Value)?.ToList();

                        elements = elements?
                            .Where(x => x.GetAttributeValue(Enums.HtmlAttribute.ClassName.Value, "").Contains(multi.ClassName.ToString()))?
                            .ToList();

                        return elements;

                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);

                        var elements = browser.Document.DocumentNode?.Descendants(multi.Value)?.ToList();
                        elements = elements?
                            .Where(x => x.GetAttributeValue(Enums.HtmlAttribute.ClassName.Value, "").Contains(multi.ClassName.ToString()))?
                            .ToList();

                        return elements?[i];
                    }
                }
                else if (multi.From == MultiNode.MultiNodeType.Tag)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        return browser.Document.DocumentNode.Descendants(node.Value)?.ToList();
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);
                        return browser.Document.DocumentNode.Descendants(node.Value)?.ToList()[i];
                    }
                }
            }
            return null;
        }

        private object GetElement(Node node, HtmlNode element)
        {
            if (node is SingleNode single)
            {
                if (single.From == SingleNode.SingleNodeType.Id)
                {
                    foreach (HtmlNode child in element.ChildNodes)
                    {
                        if (child.Id == node.Value)
                        {
                            return child;
                        }
                    }
                }
                else if (single.From == SingleNode.SingleNodeType.Index)
                {
                    return element.ChildNodes[Convert.ToInt32(node.Value)];
                }
            }
            else if (node is MultiNode multi)
            {
                if (multi.From == MultiNode.MultiNodeType.Class)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        var elements = element.Descendants(multi.Value)?.ToList();
                        elements = elements?
                            .Where(x => x.GetAttributeValue(Enums.HtmlAttribute.ClassName.Value, "").Equals(multi.ClassName))?
                            .ToList();

                        return elements;
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);

                        var elements = element.Descendants(multi.Value)?.ToList();
                        elements = elements?
                            .Where(x => x.GetAttributeValue(Enums.HtmlAttribute.ClassName.Value, "").Equals(multi.ClassName))?
                            .ToList();

                        return elements[i];
                    }
                }
                else if (multi.From == MultiNode.MultiNodeType.Tag)
                {
                    if (string.IsNullOrEmpty(multi.Index?.ToString()))
                    {
                        return element.Descendants(node.Value)?.ToList();
                    }
                    else
                    {
                        int i = Convert.ToInt32(multi.Index);
                        return element.Descendants(node.Value).ToList()[i];
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

            foreach (var multi in NodePath)
            {
                if (multi is MultiNode m)
                {
                    m.CalculateIndex();
                }
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

        public override string GetDescription()
        {
            return $"[{Name}] => ";
        }
    }

}
