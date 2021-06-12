using System;
using System.Diagnostics;
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
            Application.Run(new Tester());
        }
    }
}
