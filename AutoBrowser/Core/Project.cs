using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AutoBrowser.Core
{
    public class Project
    {
        #region Global Variables
        private string _filepath;
        #endregion

        #region Enums
        public enum Browsers
        {
            WebBrowser = 0,
            WebView = 1
        }
        #endregion

        #region Properties
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Actions.BaseAction> Actions { get; set; }
        public Browsers Browser { get; set; }
        public bool ActiveScripts { get; set; }
        #endregion

        #region Constructors
        public Project()
        {
        }

        public Project(string filePath)
        {
            _filepath = filePath;
            if (File.Exists(filePath))
            {
                Load();
            }
        }
        #endregion

        #region Functions
        public void Save()
        {
            if (string.IsNullOrEmpty(_filepath))
            {
                string fileName = (Name ?? $"Temp{DateTime.Now.ToString("hhmmss")}") + Global.FileExtension;
                _filepath = Path.Combine(Environment.CurrentDirectory, "Projects", fileName);
                _filepath =SharedLibrary.File.FormatValidFileName(_filepath);
            }

            Save(_filepath);
        }

        public void Save(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            XmlSerializer xs = new XmlSerializer(GetType(), GetSubClasses());
            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate);

            xs.Serialize(fs, this);
            fs.Close();
        }

        public string GetFilePath()
        {
            return _filepath;
        }

        private void Load()
        {
            XmlSerializer xs = new XmlSerializer(GetType(), GetSubClasses());
            FileStream fs = new FileStream(_filepath, FileMode.Open);

            var project = (Project)xs.Deserialize(fs);
            fs.Close();

            Name = project.Name;
            Description = project.Description;
            Actions = project.Actions;
            Browser = project.Browser;
            ActiveScripts = project.ActiveScripts;

            Actions.ForEach(x => x.InitVariables());
        }

        private Type[] GetSubClasses()
        {
            var allTypes = new List<Type> { typeof(List<Actions.BaseAction>) };
            allTypes.AddRange(Core.Actions.BaseAction.GetActions());
            allTypes.AddRange(Core.Actions.Node.GetSubtypes());
            return allTypes.ToArray();
        }
        #endregion

        #region Shared
        public static void Execute(string fileName)
        {
            string logFile = "";
            try
            {
                if (!File.Exists(fileName))
                {
                    return;
                }

                var project = new Project(fileName);

                var browser = new AutoWebBrowser(project.Browser, project.ActiveScripts);
                logFile = ConfigureLog(fileName, browser);
                browser.Run(project.Actions);
            }
            catch (Exception ex)
            {
               SharedLibrary.File.WriteOnFile($"[{DateTime.Now.ToLongTimeString()}] ERROR:\n{ex.Message}\n{ex.StackTrace}", logFile);
            }
        }

        private static string ConfigureLog(string fileName, AutoWebBrowser browser)
        {
            string logFile = $"{fileName.Replace(Global.FileExtension, "")}.log";
           SharedLibrary.File.WriteOnFile($"[{DateTime.Now.ToLongTimeString()}] PROCESS STARTING.", logFile);

            browser.ProgressChanged += (s, e) =>
            {
               SharedLibrary.File.WriteOnFile($"[{DateTime.Now.ToLongTimeString()}] {e.Description}", logFile);
            };
            browser.ProcessFinished += (s, e) =>
            {
               SharedLibrary.File.WriteOnFile($"[{DateTime.Now.ToLongTimeString()}] PROCESS FINISHED SUCCESSFULLY.", logFile);
            };

            return logFile;
        }

        #endregion
    }
}
