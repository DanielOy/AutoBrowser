using System.Collections.Generic;
using System.Windows.Forms;

namespace AutoBrowser.Actions
{
    public class ToastNotification : BaseAction
    {
        #region Variables
        private readonly string _originalTitle;
        private readonly string _originalBody;
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Body { get; set; }
        #endregion

        #region Constructor
        public ToastNotification(string body)
        {
            Title = _originalTitle = Application.ProductName;
            Body = _originalBody = body;
        }

        public ToastNotification(string title, string body)
        {
            Title = _originalTitle = title;
            Body = _originalBody = body;
        }
        #endregion

        public void Perform()
        {
            NotifyIcon notify = new NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
                Visible = true
            };

            notify.ShowBalloonTip(3000,
                Title,
                Body,
                ToolTipIcon.Info);
        }
        
        protected override void ResetValues()
        {
            Title = _originalTitle;
            Body = _originalBody;
        }
    }
}
