using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace SharedLibrary
{
    public static class WinRegistry
    {

        public static void SetFileAsocciation(string extension, string applicationPath)
        {
            RegistryKey extensionSubKey = Registry.CurrentUser.OpenSubKey($@"Software\Classes\{extension}\shell\open\command", true);
            string registerValue = $"{applicationPath} %1";

            if (extensionSubKey == null)
            {
                RegistryKey extensionKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{extension}");
                extensionSubKey = extensionKey.CreateSubKey(@"shell\open\command");
                extensionKey.Close();
            }

            if (!(extensionSubKey.GetValue("")?.ToString()?.Equals(registerValue) ?? false))
            {
                extensionSubKey.SetValue("", registerValue);
                extensionSubKey.Close();

                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }

            string applicationExe = applicationPath.Substring(applicationPath.LastIndexOf("\\")+1);
            var exeKey = Registry.ClassesRoot.OpenSubKey($@"Applications\{applicationExe}\shell\open\command", true);

            if (!(exeKey?.GetValue("")?.ToString()?.Equals(registerValue) ?? false))
            {
                exeKey.SetValue("", registerValue);
                exeKey.Close();

                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
