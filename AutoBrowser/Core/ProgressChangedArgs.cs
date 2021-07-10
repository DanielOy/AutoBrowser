using System;

namespace AutoBrowser.Core
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
