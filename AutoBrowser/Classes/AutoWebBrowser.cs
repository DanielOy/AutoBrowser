using AutoBrowser.Actions;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Classes
{
    //TODO: Allow to know the current step, create events.
    //TODO: Add conditions if and for
    //TODO: Allow to download files with jdownloader.
    //TODO: Create a forms to configure steps.
    //TODO: Implements sqlite.

    public class AutoWebBrowser
    {
        private WebBrowser _browser;
        private readonly Dictionary<string, object> _savedElements;
        private readonly Dictionary<string, object> _savedValues;

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

        public void Run(List<BaseAction> steps)
        {
            object result = null;
            foreach (var step in steps)
            {

                switch (step)
                {
                    case Redirect redirect:
                        //TODO: create a method to replace variable values 
                        if (redirect.Url.StartsWith("["))
                        {
                            redirect.Url = _savedValues[(redirect.Url.Replace("[", "").Replace("]", ""))].ToString();
                        }
                        redirect.Perform(_browser);
                        break;
                    case Download download:
                        if (download.Url.StartsWith("["))
                        {
                            download.Url = _savedValues[(download.Url.Replace("[", "").Replace("]", ""))].ToString();
                        }
                        download.Perform(_browser);
                        break;
                    case ExtractAttribute attribute:
                        result = attribute.Perform(_savedElements[attribute.Variable] as HtmlElement);
                        if (_savedValues.ContainsKey(attribute.Name))
                        {
                            _savedValues.Remove(attribute.Name);
                        }
                        _savedValues.Add(attribute.Name, result);
                        break;
                    case ExtractElement element:
                        result = element.Perform(_browser);
                        if (_savedElements.ContainsKey(element.Name))
                        {
                            _savedElements.Remove(element.Name);
                        }
                        _savedElements.Add(element.Name, result);
                        break;
                    case Click click:
                        click.Perform(_savedElements[click.Variable.Replace("[", "").Replace("]", "")] as HtmlElement);
                        break;
                    default:
                        result = step.Perform(_browser);
                        break;
                }
            }
        }
    }
}
