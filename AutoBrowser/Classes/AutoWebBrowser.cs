using AutoBrowser.Actions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Classes
{
    //TODO: Make the process async
    //TODO: Add wait action dynamically 
    //TODO: Implements notifications and write on file
    //TODO: Add conditions if 
    //TODO: Allow to download files with jdownloader and IDM. 
    //TODO: Create a forms to configure steps.
    //TODO: Implements sqlite or nosql

    public class AutoWebBrowser
    {
        private WebBrowser _browser;
        private readonly Dictionary<string, object> _savedElements;
        private readonly Dictionary<string, object> _savedValues;

        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedArgs e);
        public event ProgressChangedEventHandler OnProgressChanged;

        public AutoWebBrowser()
        {
            _browser = new WebBrowser() { ScriptErrorsSuppressed = true };
            _savedElements = new Dictionary<string, object>();
            _savedValues = new Dictionary<string, object>();
        }

        public AutoWebBrowser(WebBrowser browser)
        {
            _browser = browser;
            _browser.ScriptErrorsSuppressed = true;
            _browser.Navigating += _browser_Navigating;
            _browser.NewWindow += _browser_NewWindow;
            _savedElements = new Dictionary<string, object>();
            _savedValues = new Dictionary<string, object>();
        }

        private void _browser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string url = (sender as WebBrowser).Url.AbsoluteUri;
        }

        private void _browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.Host.Equals("poweredby.jads.co") || e.Url.Host.Equals("cdn.cloudimagesb.com"))
            {
                e.Cancel = true;
            }
            else //UNDO: only for tests.
            {
                string url = e.Url.AbsoluteUri;
            }

            if (e.Url.AbsoluteUri.EndsWith(".exe") || e.Url.AbsoluteUri.EndsWith(".gif"))
            {
                e.Cancel = true;
            }
        }

        private void PerformProgressChangedEvent(string description)
        {
            OnProgressChanged?.Invoke(this, new ProgressChangedArgs(description));
        }

        public void Run(List<BaseAction> steps)
        {
            PerformActions(steps);
            PerformProgressChangedEvent("Process fished.");
        }

        public void PerformActions(List<BaseAction> steps)
        {
            foreach (var step in steps)
            {
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
                        download.Perform(_browser);
                        break;
                    case ExtractAttribute attribute:
                        PerformProgressChangedEvent($"Getting {attribute.AttributeName} from {attribute.Variable}");
                        attribute.ReplaceVariables(_savedValues);
                        result = IsElementCollection(attribute.Variable) ?
                            attribute.Perform(GetElementCollection(attribute.Variable)) :
                            IsElementList(attribute.Variable) ? attribute.Perform(GetElementList(attribute.Variable)) :
                            attribute.Perform(GetElement(attribute.Variable));
                        SaveAttribute(attribute.Name, result);
                        break;
                    case ExtractElement element:
                        PerformProgressChangedEvent($"Getting {element.Name}");
                        element.ReplaceVariables(_savedValues);
                        result = element.Perform(_browser);
                        SaveElement(element.Name, result);
                        break;
                    case Click click:
                        click.ReplaceVariables(_savedValues);
                        PerformProgressChangedEvent($"Perform click on {click.Variable} element");
                        click.Perform(GetElement(click.Variable));
                        break;
                    case WebAction web:
                        PerformProgressChangedEvent($"Executing {web.Action.ToString()}");
                        result = web.Perform(_browser);
                        break;
                    case Repeat f:
                        f.ReplaceVariables(_savedValues);
                        int times = Convert.ToInt32(f.Times);
                        for (int i = 0; i < times; i++)
                        {
                            SaveAttribute(f.Name, f.Calculate(i));
                            PerformActions(f.Actions);
                        }
                        RemoveAttribute(f.Name);
                        break;
                }
            }
        }

        private List<HtmlElement> GetElementList(string name)
        {
            return _savedElements[name] as List<HtmlElement>;
        }

        private bool IsElementList(string name)
        {
            return _savedElements[name] is List<HtmlElement>;
        }

        private HtmlElementCollection GetElementCollection(string name)
        {
            return _savedElements[name] as HtmlElementCollection;
        }

        private bool IsElementCollection(string name)
        {
            return _savedElements[name] is HtmlElementCollection;
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

        private HtmlElement GetElement(string name)
        {
            return _savedElements[name] as HtmlElement;
        }

        private void SaveElement(string elementName, object result)
        {
            if (result == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(elementName))
            {
                return;
            }

            if (_savedElements.ContainsKey(elementName))
            {
                _savedElements.Remove(elementName);
            }
            _savedElements.Add(elementName, result);
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
