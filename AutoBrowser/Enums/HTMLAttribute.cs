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

        private HtmlAttribute(string value)
        {
            Value = value;
        }

        public static HtmlAttribute href { get { return new HtmlAttribute("href"); } }
        public static HtmlAttribute alt { get { return new HtmlAttribute("alt"); } }
        public static HtmlAttribute length { get { return new HtmlAttribute("length"); } }
        public static HtmlAttribute ClassName { get { return new HtmlAttribute("className"); } }
        public static HtmlAttribute title { get { return new HtmlAttribute("title"); } }
        public static HtmlAttribute src { get { return new HtmlAttribute("src"); } }
        public static HtmlAttribute poster { get { return new HtmlAttribute("poster"); } }
    }
}
