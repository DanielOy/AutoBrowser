using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using AutoBrowser.Core;
using AutoBrowser.Forms;

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
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Library.Web.ConfigureIEBrowserEmulator(Process.GetCurrentProcess().ProcessName);
            Library.WinRegistry.SetFileAsocciation(Global.FileExtension, Application.ExecutablePath);
            ChooseProcess();
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            Debug.WriteLine(e.Exception.Message);
            Library.File.WriteOnFile($"{e.Exception.Message}\n{e.Exception.StackTrace}", "Error.log");
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            Debug.WriteLine((e.ExceptionObject as Exception).Message);
            Library.File.WriteOnFile($"{(e.ExceptionObject as Exception).Message}\n{(e.ExceptionObject as Exception).StackTrace}", "Error.log");
        }

        private static void ChooseProcess()
        {
            List<string> ParametersList = Environment.GetCommandLineArgs().ToList();
            if (ParametersList.Count == 1)
            {
                Application.Run(new Main());
            }
            else if (ParametersList[1].EndsWith(Global.FileExtension))
            {
                if (Environment.CurrentDirectory == Environment.SystemDirectory)
                {
                    Environment.CurrentDirectory = new System.IO.FileInfo(ParametersList[1]).DirectoryName;
                }

                if (ParametersList.Contains("--hide") || ParametersList.Contains("--h"))
                {
                    new Project().Execute(ParametersList[1]);
                }
                else
                {
                    var frm = new Tester { FileName = ParametersList[1], IsAuto = true };
                    Application.Run(frm);
                }
            }
        }
    }
}
