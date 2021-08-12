using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutoBrowser.Core.Browsers
{
    public abstract class BaseBrowser
    {
        #region Global Variables
        private List<string> _blackList;
        private const string _blackListFile = "BlackList.dat";
        #endregion

        #region Properties
        public abstract Uri Url { get;}
        public abstract string Cookies { get; }
        public abstract HtmlAgilityPack.HtmlDocument Document { get; }
        #endregion

        #region Constructors
        public BaseBrowser()
        {
            LoadBlackList();
        }
        #endregion

        #region Functions
        public abstract void Navigate(string url, int timeOut);
        #endregion

        #region Shared Functions
        private void LoadBlackList()
        {
            if (!System.IO.File.Exists(_blackListFile))
            {
                System.IO.File.WriteAllLines(_blackListFile, new string[]{
                    "poweredby.jads.co",
                    "cdn.cloudimagesb.com",
                    "googleads.g.doubleclick.net"});
            }

            _blackList = System.IO.File.ReadAllLines(_blackListFile)
                .Where(x => !string.IsNullOrEmpty(x.Trim()))
                .ToList();
        }

        protected bool IsAdLink(Uri url)
        {
            return _blackList.Contains(url.Host);
        }

        protected void Wait(int seconds)
        {
            DateTime finalTime = DateTime.Now.AddSeconds(seconds);

            while (finalTime > DateTime.Now)
            {
                Application.DoEvents();
            }
        }
        #endregion
    }
}
