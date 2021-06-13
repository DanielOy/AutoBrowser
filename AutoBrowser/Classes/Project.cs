using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace AutoBrowser.Classes
{
    public class Project
    {
        private string _logFile;

        public void Execute(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    return;
                }

                var actions = LoadProject(fileName);

                var browser = new AutoWebBrowser();
                ConfigureLog(fileName, browser);
                browser.Run(actions);
            }
            catch (Exception ex)
            {
                Library.File.WriteOnFile($"[{DateTime.Now.ToLongTimeString()}] ERROR:\n{ex.Message}\n{ex.StackTrace}", _logFile);
            }
        }

        private void ConfigureLog(string fileName, AutoWebBrowser browser)
        {
            _logFile = $"{fileName.Replace(".aweb", "")}_{DateTime.Now.ToString("yyMMdd_hhmmss")}.log";
            Library.File.WriteOnFile($"[{DateTime.Now.ToLongTimeString()}] PROCESS STARTING.", _logFile);

            browser.ProgressChanged += (s, e) =>
            {
                Library.File.WriteOnFile($"[{DateTime.Now.ToLongTimeString()}] {e.Description}", _logFile);
            };
            browser.ProcessFinished += (s, e) =>
            {
                Library.File.WriteOnFile($"[{DateTime.Now.ToLongTimeString()}] PROCESS FINISHED SUCCESSFULLY.", _logFile);
            };
        }

        public List<Actions.BaseAction> LoadProject(string fileName)
        {
            XmlSerializer xs = new XmlSerializer(typeof(List<Actions.BaseAction>), GetSubClasses());
            FileStream fs = new FileStream(fileName, FileMode.Open);

            List<Actions.BaseAction> actions = (List<Actions.BaseAction>)xs.Deserialize(fs);
            fs.Close();

            actions.ForEach(x => x.InitVariables());

            return actions;
        }

        public void SaveProject(List<Actions.BaseAction> actions, string fileName)
        {
            XmlSerializer xs = new XmlSerializer(actions.GetType(), GetSubClasses());
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);

            xs.Serialize(fs, actions);
            fs.Close();
        }

        private Type[] GetSubClasses()
        {
            var subTypes = Assembly
                .GetAssembly(typeof(Actions.BaseAction))
                .GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract &&
                (myType.IsSubclassOf(typeof(Actions.BaseAction)) || myType.IsSubclassOf(typeof(Actions.Node))))
                .ToArray();

            return subTypes;
        }
    }
}
