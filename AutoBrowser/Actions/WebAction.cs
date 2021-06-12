using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public abstract class WebAction : BaseAction
    {
        public abstract Action Action { get; }

        public abstract object Perform(WebBrowser browser);
    }
    public enum Action
    {
        Redirect,
        ExtractElement,
        ExtractAtribute,
        Download,
        Click
    }
}
