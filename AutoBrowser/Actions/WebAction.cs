using System.Runtime.Serialization;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public abstract class WebAction : BaseAction
    {
        public abstract object Perform(WebBrowser browser);
    }
}
