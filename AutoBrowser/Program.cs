using AutoBrowser.Core;
using AutoBrowser.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            SuscribeNotifications();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            SharedLibrary.Web.ConfigureIEBrowserEmulator(Process.GetCurrentProcess().ProcessName);
            SharedLibrary.WinRegistry.SetFileAsocciation(Global.FileExtension, Application.ExecutablePath);
            ChooseProcess();
        }

        private static void SuscribeNotifications()
        {
            ToastNotificationManagerCompat.OnActivated += toastArgs =>
            {
                ToastArguments args = ToastArguments.Parse(toastArgs.Argument);

                if (!string.IsNullOrEmpty(args.Get("process")))
                    Process.Start(args.Get("process"));
            };
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            Debug.WriteLine(e.Exception.Message);
            SharedLibrary.File.WriteOnFile($"{e.Exception.Message}\n{e.Exception.StackTrace}", "Error.log");
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Log the exception, display it, etc
            Debug.WriteLine((e.ExceptionObject as Exception).Message);
            SharedLibrary.File.WriteOnFile($"{(e.ExceptionObject as Exception).Message}\n{(e.ExceptionObject as Exception).StackTrace}", "Error.log");
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
                Environment.CurrentDirectory = new System.IO.FileInfo(ParametersList[1]).DirectoryName;

                if (ParametersList.Contains("--hide") || ParametersList.Contains("-h"))
                {
                    Project.Execute(ParametersList[1]);
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
