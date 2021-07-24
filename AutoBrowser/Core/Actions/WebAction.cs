using AutoBrowser.Core.Browsers;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace AutoBrowser.Core.Actions
{
    public abstract class WebAction : BaseAction
    {
        public abstract object Perform(BaseBrowser browser);
    }
}
