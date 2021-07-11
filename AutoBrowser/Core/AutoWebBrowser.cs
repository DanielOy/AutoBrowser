﻿using AutoBrowser.Core.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoBrowser.Core
{
    //TODO: V2: Implements sqlite or nosql
    //TODO: V2: Improve notifications
    //TODO: Make the process async
    //TODO: Allow user to generate a task in task scheduler
    //TODO: Generate a library for get access to task scheduler : https://docs.microsoft.com/en-gb/windows/win32/taskschd/schtasks?redirectedfrom=MSDN

    public class AutoWebBrowser
    {
        private WebBrowser _browser;
        private List<string> _blackList;
        private readonly Dictionary<string, object> _savedElements;
        private readonly Dictionary<string, object> _savedValues;
        private const string _blackListFile = "BlackList.dat";

        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedArgs e);
        public delegate void ProcessFinishedEventHandler(object sender, EventArgs e);
        public event ProgressChangedEventHandler ProgressChanged;
        public event ProcessFinishedEventHandler ProcessFinished;

        public AutoWebBrowser()
        {
            _browser = new WebBrowser() { ScriptErrorsSuppressed = true };
            _browser.Navigating += _browser_Navigating;
            _browser.NewWindow += _browser_NewWindow;
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
            LoadBlackList();
        }

        private void LoadBlackList()
        {
            if (!System.IO.File.Exists(_blackListFile))
            {
                System.IO.File.WriteAllLines(_blackListFile, new string[]{
                    "poweredby.jads.co",
                    "cdn.cloudimagesb.com",
                    "googleads.g.doubleclick.net"});
            }

            _blackList = System.IO.File.ReadAllLines(_blackListFile)
                .Where(x => !string.IsNullOrEmpty(x.Trim()))
                .ToList();
        }

        private void _browser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void _browser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (IsAdLink(e.Url))
            {
                e.Cancel = true;
            }
            else if (e.Url.AbsoluteUri.EndsWith(".exe"))
            {
                e.Cancel = true;
            }
            else if (string.IsNullOrEmpty(e.Url.Host))
            {
                e.Cancel = true;
            }
            else
            {
                Library.File.WriteOnFile($"{e.Url.Host}|{e.Url.AbsoluteUri}", "Pages.dat");
            }
        }

        private bool IsAdLink(Uri url)
        {
            return _blackList.Contains(url.Host);
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
                        int times = Convert.ToInt32(f.Times);
                        for (int i = 0; i < times; i++)
                        {
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