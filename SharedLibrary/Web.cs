using Microsoft.Win32;
using System;
using System.Net;
using System.Text;

namespace SharedLibrary
{
    public static class Web
    {
        [System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true, PreserveSig = true)]
        public static extern bool InternetGetCookieEx(string url, string cookieName, StringBuilder cookieData, ref int size, Int32 dwFlags, IntPtr lpReserved);

        [System.Runtime.InteropServices.DllImport("wininet.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

        public static string GetCookie(string url)
        {
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);
            if (InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero) && cookieData.Length > 0)
            {
                return cookieData.ToString();
            }
            return null;
        }

        public static CookieContainer GetCookieContainer(string url)
        {
            CookieContainer cookies = null;
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);

            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                {
                    return null;
                }

                //second attempt
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(url,null, cookieData,ref datasize,0x2000,IntPtr.Zero))
                {
                    return null;
                }
            }

            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(new Uri(url), cookieData.ToString().Replace(';', ','));
            }
            return cookies;
        }

        public static void ConfigureIEBrowserEmulator(string applicationName)
        {

            // Set the actual key                        
            var Key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
            if (Key == null)
            {
                Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
            }

            Key.SetValue($"{applicationName}.exe", "11001", RegistryValueKind.DWord);
            Key.Close();
        }
    }
}
