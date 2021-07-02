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
            string keyValue = applicationPath + " %1";

            if (subKey == null)
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey("Software\\Classes\\" + extension);
                subKey = key.CreateSubKey("shell\\open\\command");
                key.Close();
            }

            if (!(subKey.GetValue("")?.ToString()?.Equals(keyValue) ?? false))
            {
                subKey.SetValue("", keyValue);
                subKey.Close();

                SHChangeNotify(0x08000000, 0x0000, IntPtr.Zero, IntPtr.Zero);
            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
