using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBrowser.Enums
{
    public class HtmlAttribute
    {
        public string Value { get; set; }

        public HtmlAttribute(string value)
        {
            Value = value;
        }

        public static HtmlAttribute href { get { return new HtmlAttribute("href"); } }
        public static HtmlAttribute alt { get { return new HtmlAttribute("alt"); } }
        public static HtmlAttribute length { get { return new HtmlAttribute("length"); } }
    }
}
