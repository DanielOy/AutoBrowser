using AutoBrowser.Core.Browsers;
using System.Windows.Forms;

namespace AutoBrowser.Core.Actions
{
    public class Wait : WebAction
    {
        private string _originalElement;

        public enum WaitFunctions
        {
            NoSeconds = 0,
            LoadElement = 1,
        }

        public WaitFunctions SpecialFunction { get; set; } = WaitFunctions.NoSeconds;
        public int TimeOut { get; set; }
        public string Element { get; set; }

        public Wait() { }
        public Wait(WaitFunctions function, int timeOut, string element)
        {
            SpecialFunction = function;
            TimeOut = timeOut;
            Element = _originalElement = element;
        }

        public override object Perform(BaseBrowser browser)
        {
            if (SpecialFunction == WaitFunctions.NoSeconds)
            {
                Wait(TimeOut);
            }
            else if (SpecialFunction == WaitFunctions.LoadElement)
            {
                WaitLoadElement(browser);
            }
            return true;
        }

        private void WaitLoadElement(BaseBrowser browser)
        {
            int secondsWaited = 0;
            if (browser == null || browser.Document == null)
            {
                while ((browser == null || browser.Document == null) && TimeOut > secondsWaited)
                {
                    Wait(1);
                    secondsWaited++;
                }
            }

            secondsWaited = 0;
            while (browser.Document==null && browser.Document.GetElementbyId(Element) == null && TimeOut > secondsWaited)
            {
                Wait(1);
                secondsWaited++;
            }
        }

        internal override void InitVariables()
        {
            _originalElement = Element;
        }

        public override string GetDescription()
        {
            return $"Wait <{SpecialFunction}> <{TimeOut}>";
        }

        protected override void ResetValues()
        {
            Element = _originalElement;
        }
    }
}
