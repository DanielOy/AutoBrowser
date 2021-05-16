using System;

namespace AutoBrowser.Classes
{
    public class ProgressChangedArgs : EventArgs
    {
        public ProgressChangedArgs(string description)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
