using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AutoBrowser.Actions
{
    public abstract class BaseAction
    {
        internal virtual void ReplaceVariables(Dictionary<string, object> sessionVariables)
        {
            if (sessionVariables == null || sessionVariables.Count == 0)
            {
                return;
            }

            ResetValues();

            Type actionType = GetType();
            List<PropertyInfo> properties = new List<PropertyInfo>(actionType.GetProperties())
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
    }
}
