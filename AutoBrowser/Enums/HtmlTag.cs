using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoBrowser.Enums
{
    public class HtmlTag
    {
        public string Value { get; set; }

        public HtmlTag(string value)
        {
            Value = value;
        }

        public static HtmlTag img { get { return new HtmlTag("img"); } }
        public static HtmlTag figure { get { return new HtmlTag("figure"); } }
        public static HtmlTag a { get { return new HtmlTag("a"); } }
        public static HtmlTag div { get { return new HtmlTag("div"); } }
        public static HtmlTag source { get { return new HtmlTag("source"); } }
        public static HtmlTag video { get { return new HtmlTag("video"); } }
    }
}
