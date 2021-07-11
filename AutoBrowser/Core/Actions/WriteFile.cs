namespace AutoBrowser.Core.Actions
{
    public class WriteFile : BaseAction
    {
        #region Global Variables
        private string _originalText;
        private string _originalFileName;
        #endregion

        #region Properties
        public string Text { get; set; }

        public string FileName { get; set; }
        #endregion

        #region Constructor
        public WriteFile() { }

        public WriteFile(string text, string fileName)
        {
            Text = _originalText = text;
            FileName = _originalFileName = fileName;
        } 
        #endregion

        protected override void ResetValues()
        {
            Text = _originalText;
            FileName = _originalFileName;
        }

        public void Perform()
        {
            Library.File.WriteOnFile(Text, FileName);
        }

        internal override void InitVariables()
        {
            _originalText = Text;
            _originalFileName = FileName;
        }

        public override string GetDescription()
        {
            return $"Write <{Text}> on <{FileName}>";
        }
    }
}
