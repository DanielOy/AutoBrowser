using AutoBrowser.Core.Actions;
using AutoBrowser.Core.Browsers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;


namespace AutoBrowser.Core
{
    //TODO: V2: Implements sqlite or nosql
    //TODO: V2: Improve notifications
    //TODO: V2: Allow user to generate a task in task scheduler
    //TODO: Make the process async

    public class AutoWebBrowser
    {
        private readonly BaseBrowser _browser;
        private readonly Dictionary<string, object> _savedNodes;
        private readonly Dictionary<string, object> _savedValues;
        private bool canContinue = true;

        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedArgs e);
        public delegate void ProcessFinishedEventHandler(object sender, EventArgs e);
        public event ProgressChangedEventHandler ProgressChanged;
        public event ProcessFinishedEventHandler ProcessFinished;

        public AutoWebBrowser()
        {
            _browser = new WBrowser();
            _savedNodes = new Dictionary<string, object>();
            _savedValues = new Dictionary<string, object>();
        }

        public AutoWebBrowser(Project.Browsers browser, bool activeScripts)
        {
            switch (browser)
            {
                case Project.Browsers.WebView:
                    _browser = new WView(activeScripts);
                    break;
                default:
                    _browser = new WBrowser();
                    break;
            }
            _savedNodes = new Dictionary<string, object>();
            _savedValues = new Dictionary<string, object>();
        }

        public AutoWebBrowser(System.Windows.Forms.WebBrowser browser)
        {
            _browser = new WBrowser(browser);
            _savedNodes = new Dictionary<string, object>();
            _savedValues = new Dictionary<string, object>();
        }

        public AutoWebBrowser(Microsoft.Web.WebView2.WinForms.WebView2 view, bool activeScripts)
        {
            _browser = new WView(view,activeScripts);
            _savedNodes = new Dictionary<string, object>();
            _savedValues = new Dictionary<string, object>();
        }

        private void PerformProgressChangedEvent(string description)
        {
            ProgressChanged?.Invoke(this, new ProgressChangedArgs(description));
        }

        private void PerformProcessFinishedEvent()
        {
            ProcessFinished?.Invoke(this, new EventArgs());
        }

        public void Run(List<BaseAction> steps)
        {
            PerformActions(steps);
            PerformProcessFinishedEvent();
        }

        public void PerformActions(List<BaseAction> steps)
        {
            foreach (var step in steps)
            {
                if (!canContinue) { return; }

                object result = null;
                switch (step)
                {
                    case Redirect redirect:
                        redirect.ReplaceVariables(_savedValues);
                        PerformProgressChangedEvent($"Navigating to {redirect.Url}");
                        redirect.Perform(_browser);
                        break;
                    case BaseDownload download:
                        download.ReplaceVariables(_savedValues);
                        PerformProgressChangedEvent($"Downloading from: {download.Url} to: {download.FileName}");
                        if (download is Download dw) { dw.ProgressChanged += ProgressChanged; }
                        download.Perform(_browser);
                        break;
                    case ExtractAttribute attribute:
                        PerformProgressChangedEvent($"Getting {attribute.AttributeName} from {attribute.Variable}");
                        attribute.ReplaceVariables(_savedValues);
                        result = IsNodeCollection(attribute.Variable) ?
                            attribute.Perform(GetNodeCollection(attribute.Variable)) :
                            IsNodeList(attribute.Variable) ? attribute.Perform(GetNodeList(attribute.Variable)) :
                            attribute.Perform(GetNode(attribute.Variable));
                        SaveAttribute(attribute.Name, result);
                        break;
                    case ExtractElement element:
                        PerformProgressChangedEvent($"Getting {element.Name}");
                        element.ReplaceVariables(_savedValues);
                        result = element.Perform(_browser);
                        SaveNode(element.Name, result);
                        break;
                    case Click click:
                        click.ReplaceVariables(_savedValues);
                        PerformProgressChangedEvent($"Perform click on {click.Variable} element");
                        click.Perform(GetNode(click.Variable));
                        break;
                    case WebAction web:
                        PerformProgressChangedEvent($"Performing: {web.GetDescription()}");
                        result = web.Perform(_browser);
                        break;
                    case Input input:
                        PerformProgressChangedEvent($"Waiting for user input");
                        result = input.Perform();
                        SaveAttribute(input.Name, result);
                        break;
                    case WriteFile write:
                        write.ReplaceVariables(_savedValues);
                        PerformProgressChangedEvent($"Writing text in file.");
                        write.Perform();
                        break;
                    case ToastNotification toast:
                        PerformProgressChangedEvent($"Launching toast notification");
                        toast.ReplaceVariables(_savedValues);
                        toast.Perform();
                        break;
                    case Repeat f:
                        f.ReplaceVariables(_savedValues);
                        string _times = f.Times.ToString().Trim().Replace(",", "");
                        int times = Convert.ToInt32(_times);
                        for (int i = 0; i < times; i++)
                        {
                            if (!canContinue) { return; }
                            SaveAttribute(f.Name, f.Calculate(i));
                            PerformActions(f.Actions);
                        }
                        RemoveAttribute(f.Name);
                        break;
                    case Conditional c:
                        c.ReplaceVariables(_savedValues);
                        if (c.EvaluateCondition())
                        {
                            PerformActions(c.Actions);
                        }
                        break;
                    case ExternalProcess ep:
                        ep.ReplaceVariables(_savedValues);
                        PerformProgressChangedEvent(ep.GetDescription());
                        ep.Perform();
                        break;
                }
            }
        }

        public void Stop()
        {
            canContinue = false;
        }

        private List<HtmlNode> GetNodeList(string name)
        {
            return _savedNodes[name] as List<HtmlNode>;
        }

        private bool IsNodeList(string name)
        {
            return _savedNodes[name] is List<HtmlNode>;
        }

        private HtmlNodeCollection GetNodeCollection(string name)
        {
            return _savedNodes[name] as HtmlNodeCollection;
        }

        private bool IsNodeCollection(string name)
        {
            return _savedNodes[name] is HtmlNodeCollection;
        }

        private void RemoveAttribute(string name)
        {
            if (_savedValues == null || _savedValues.Count == 0)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }
            _savedValues.Remove(name);
        }

        private HtmlNode GetNode(string name)
        {
            return _savedNodes[name] as HtmlNode;
        }

        private void SaveNode(string elementName, object result)
        {
            if (result == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(elementName))
            {
                return;
            }

            if (_savedNodes.ContainsKey(elementName))
            {
                _savedNodes.Remove(elementName);
            }
            _savedNodes.Add(elementName, result);
        }

        private void SaveAttribute(string attributeName, object result)
        {
            if (result == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(attributeName))
            {
                return;
            }

            if (_savedValues.ContainsKey(attributeName))
            {
                _savedValues.Remove(attributeName);
            }
            _savedValues.Add(attributeName, result);
        }
    }
}
