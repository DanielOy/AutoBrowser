using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows.Forms;

namespace AutoBrowser.Core.Actions
{
    public class ToastNotification : BaseAction
    {
        #region Global Variables
        private string _originalTitle;
        private string _originalBody;
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Body { get; set; }
        #endregion

        #region Constructors
        public ToastNotification() { }
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

        #region Functions
        public void Perform()
        {
            //NotifyIcon notify = new NotifyIcon
            //{
            //    Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location),
            //    Visible = true
            //};

            //notify.ShowBalloonTip(3000,
            //    Title,
            //    Body,
            //    ToolTipIcon.Info);

            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddText(Title)
                .AddText(Body)
                .Show();
        }

        protected override void ResetValues()
        {
            Title = _originalTitle;
            Body = _originalBody;
        }

        internal override void InitVariables()
        {
            _originalTitle = Title;
            _originalBody = Body;
        }

        public override string GetDescription()
        {
            return $"Notify <{Title}>";
        }
        #endregion
    }
}
