using System.Collections.Generic;

namespace AutoBrowser.Actions
{
    public abstract class BaseAction
    {
        public abstract void ReplaceVariables(Dictionary<string, object> savedValues);

        protected abstract void ResetValues();
    }
}
