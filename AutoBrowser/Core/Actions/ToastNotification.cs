using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows.Forms;

namespace AutoBrowser.Core.Actions
{
    public class ToastNotification : BaseAction
    {
        #region Global Variables
        private string _originalTitle;
        private string _originalBody;
        private string _originalProcess;
        #endregion

        #region Properties
        public string Title { get; set; }
        public string Body { get; set; }
        public string Process { get; set; }
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

        public ToastNotification(string title, string body, string process)
        {
            Title = _originalTitle = title;
            Body = _originalBody = body;
            Process = _originalProcess = process;
        }
        #endregion

        #region Functions
        public void Perform()
        {
            new ToastContentBuilder()
                .AddArgument("action", "viewConversation")
                .AddArgument("conversationId", 9813)
                .AddArgument("process", Process)
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
