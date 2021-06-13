using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public abstract class BaseAction
    {
        internal abstract void InitVariables();

        internal virtual void ReplaceVariables(Dictionary<string, object> sessionVariables)
        {
            if (sessionVariables == null || sessionVariables.Count == 0)
            {
                return;
            }

            ResetValues();

            List<PropertyInfo> properties = new List<PropertyInfo>(GetType().GetProperties())
                .Where(x => x.PropertyType == typeof(string) || x.PropertyType == typeof(object))?
                .ToList();

            if (properties == null || properties.Count == 0)
            {
                return;
            }

            foreach (var variable in sessionVariables)
            {
                foreach (PropertyInfo propertie in properties)
                {
                    string propValue = propertie.GetValue(this)?.ToString();

                    if (propValue.Contains($"[{variable.Key}]"))
                    {
                        propertie.SetValue(this, propValue.Replace($"[{variable.Key}]", variable.Value.ToString()));
                    }
                }
            }
        }

        protected abstract void ResetValues();

        protected void Wait(int seconds)
        {
            DateTime finalTime = DateTime.Now.AddSeconds(seconds);

            while (finalTime > DateTime.Now)
            {
                Application.DoEvents();
            }
        }
    }
}
