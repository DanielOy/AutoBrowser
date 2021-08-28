using SharedLibrary.Forms;
using System.Windows.Forms;

namespace AutoBrowser.Core.Actions
{
    public class Input : BaseAction
    {
        private string _originalName = "";
        private string _originalDescription = "";

        public string Name { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; } = "";
        public SpecialFormat Format { get; set; } = SpecialFormat.None;

        public enum SpecialFormat
        {
            None,
            URLEncoding
        }

        public Input()
        {
        }

        public Input(string name)
        {
            Name = _originalName = name;
        }

        public Input(string name, string description)
        {
            Name = _originalName = name;
            Description = _originalDescription = description;
        }

        public string Perform()
        {
            string description = string.IsNullOrEmpty(Description) ? "Input" : Description;

            if (InputBox.Show($"Please insert a value for [{description}]", description, out string input) == DialogResult.OK)
            {
                return FormatString(input);
            }
            return DefaultValue;
        }

        private string FormatString(string input)
        {
            switch (Format)
            {
                case SpecialFormat.URLEncoding: return SharedLibrary.TextFormat.URLEnconde(input);
                default: return input;
            }
        }

        protected override void ResetValues()
        {
            Name = _originalName;
            Description = _originalDescription;
        }

        internal override void InitVariables()
        {
            _originalName = Name;
            _originalDescription = Description;
        }

        public override string GetDescription()
        {
            return $"Request a value for <{Name}>";
        }
    }
}
