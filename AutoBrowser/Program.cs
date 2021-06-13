using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace AutoBrowser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Library.Web.ConfigureIEBrowserEmulator(Process.GetCurrentProcess().ProcessName);
            Library.WinRegistry.SetFileAsocciation(Global.FileExtension, Application.ExecutablePath);
            ChooseProcess();
        }

        private static void ChooseProcess()
        {
            List<string> ParametersList = Environment.GetCommandLineArgs().ToList();
            if (ParametersList.Count == 1)
            {
                Application.Run(new Tester());
            }
            else if (ParametersList[1].EndsWith(Global.FileExtension))
            {
                MessageBox.Show(Environment.CurrentDirectory +"\n\n"+ Environment.SystemDirectory);
                if (Environment.CurrentDirectory == Environment.SystemDirectory)
                {
                    Environment.CurrentDirectory = new System.IO.FileInfo(ParametersList[1]).DirectoryName;
                }

                if (ParametersList.Contains("--hide") || ParametersList.Contains("--h"))
                {
                    new Classes.Project().Execute(ParametersList[1]);
                }
                else
                {
                    var frm = new Tester { FileName = ParametersList[1] };
                    Application.Run(frm);
                }
            }
        }
    }
}
