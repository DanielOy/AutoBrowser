using AutoBrowser.Actions;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Classes
{
    //TODO: Add conditions if and for
    //TODO: Allow to download files with jdownloader.
    //TODO: Create a forms to configure steps.
    //TODO: Implements sqlite.

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
            _savedElements = new Dictionary<string, object>();
            _savedValues = new Dictionary<string, object>();
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
                    case Download download:
                        download.ReplaceVariables(_savedValues);
                        PerformProgressChangedEvent($"Downloading from: {download.Url} to: {download.FileName}");
                        download.Perform(_browser);
                        break;
                    case ExtractAttribute attribute:
                        PerformProgressChangedEvent($"Getting {attribute.AttributeName} from {attribute.Variable}");
                        attribute.ReplaceVariables(_savedValues);
                        result = attribute.Perform(GetElement(attribute.Variable));
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
                    case ForStructure f:
                        f.ReplaceVariables(_savedValues);
                        int times = Convert.ToInt32(f.Times);
                        for (int i = 0; i < times; i++)
                        {
                            SaveAttribute(f.Name, i);
                            PerformActions(f.Actions);
                        }
                        break;
                }
            }
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
