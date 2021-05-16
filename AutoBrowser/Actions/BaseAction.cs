using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public abstract class BaseAction
    {
        public abstract WebAction Action { get; }

        public abstract object Perform(WebBrowser browser);
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
