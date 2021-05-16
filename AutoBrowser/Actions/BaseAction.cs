using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public abstract class BaseAction
    {
        public abstract WebAction Action { get; }

        public abstract object Perform(WebBrowser browser);

        public virtual void ReplaceVariables(Dictionary<string, object> savedValues)
        {
            return;
        }
    }
    public enum WebAction
    {
        Redirect,
        ExtractElement,
        ExtractAtribute,
        Download,
        Click
    }
}
