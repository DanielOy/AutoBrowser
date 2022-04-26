using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AutoBrowser.Core.Actions
{
    public abstract class BaseAction
    {
        internal abstract void InitVariables();

        public abstract string GetDescription();

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

                    if (propValue != null && propValue.Contains($"[{variable.Key}]"))
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

        public static Type[] GetActions()
        {
            return Assembly
                .GetAssembly(typeof(BaseAction))
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract &&
                (myType.IsSubclassOf(typeof(BaseAction))))
                .ToArray();
        }

        public static string[] GetActionNames()
        {
            return GetActions()
                .Select(x => x.Name)
                .ToArray();
        }

        public static List<BaseAction> Copy(List<BaseAction> actions)
        {
            string tempFile = "TempFile.aweb";
            DeleteFile(tempFile);

            var Project = new Project { Actions = actions };
            Project.Save(tempFile);

            var copyActions = new Project(tempFile)?.Actions;
            DeleteFile(tempFile);

            return copyActions;
        }

        private static void DeleteFile(string tempFile)
        {
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
