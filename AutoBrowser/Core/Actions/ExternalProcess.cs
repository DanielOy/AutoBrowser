using System.IO;

namespace AutoBrowser.Core.Actions
{
    public class ExternalProcess : BaseAction
    {
        #region Global Variables
        private string _originalApplicationExe;
        private string _originalArgs;
        #endregion

        #region Properties
        public string ApplicationExe { get; set; }
        public string Args { get; set; }
        #endregion


        #region Constructors
        public ExternalProcess() { }

        public ExternalProcess(string applicationExe, string args)
        {
            ApplicationExe = _originalApplicationExe = applicationExe;
            Args = _originalArgs = args;
        }
        #endregion

        public void Perform()
        {
            if (string.IsNullOrEmpty(ApplicationExe))
            {
                return;
            }

            if (!File.Exists(ApplicationExe))
            {
                return;
            }

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = new FileInfo(ApplicationExe).FullName;
            process.StartInfo.Arguments = Args;
            process.Start();
        }

        public override string GetDescription()
        {
            return $"Execute <{ApplicationExe}> <{Args}>";
        }

        protected override void ResetValues()
        {
            ApplicationExe = _originalApplicationExe;
            Args = _originalArgs;
        }

        internal override void InitVariables()
        {
            _originalApplicationExe = ApplicationExe;
            _originalArgs = Args;
        }
    }
}
