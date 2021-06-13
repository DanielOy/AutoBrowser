using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace AutoBrowser.Library
{
    public static class WinRegistry
    {

        public static void SetFileAsocciation(string extension, string applicationPath)
        {
            RegistryKey subKey = Registry.CurrentUser.OpenSubKey($@"Software\Classes\{extension}\shell\open\command", true);
            if (subKey == null)
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + extension);
                subKey = key.CreateSubKey("shell\\open\\command");
                key.Close();
            }
            subKey.SetValue("", applicationPath + " %1");
            subKey.Close();

            SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
