namespace AutoBrowser.Actions
{
    public class WriteFile : BaseAction
    {
        private string _originalText;
        private string _originalFileName;

        public string Text { get; set; }

        public string FileName { get; set; }

        public WriteFile() { }

        public WriteFile(string text, string fileName)
        {
            Text = _originalText = text;
            FileName = _originalFileName = fileName;
        }

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
    }
}
