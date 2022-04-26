using AutoBrowser.Core.Browsers;
using System;
using System.Windows.Forms;
using static AutoBrowser.Core.AutoWebBrowser;

namespace AutoBrowser.Core.Actions
{
    public class Wait : WebAction
    {
        private string _originalElement;
        private bool _canContinue;
        private Button _button;

        public event ProgressChangedEventHandler ProgressChanged;

        public enum WaitFunctions
        {
            NoSeconds = 0,
            LoadElement = 1,
            User = 2
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
            return Perform(browser, null);
        }

        public object Perform(BaseBrowser browser, Button auxButton)
        {
            _button = auxButton;
            _canContinue = false;

            if (SpecialFunction == WaitFunctions.NoSeconds)
            {
                Wait(TimeOut);
            }
            else if (SpecialFunction == WaitFunctions.LoadElement)
            {
                WaitLoadElement(browser);
            }
            else if (SpecialFunction == WaitFunctions.User)
            {
                _button.Text = "Continue";
                _button.Visible = true;
                _button.Click += (s, e) => _canContinue = true;

                WaitForUser(TimeOut);

                _button.Text = "";
                _button.Visible = false;
            }
            return true;
        }

        private void WaitForUser(int seconds)
        {
            DateTime finalTime = DateTime.Now.AddSeconds(seconds);

            while (finalTime > DateTime.Now && !_canContinue)
            {
                int secondsRemaining = (int)(finalTime - DateTime.Now).TotalSeconds;
                ProgressChanged?.Invoke(this, new ProgressChangedArgs($"Waiting, {secondsRemaining} seconds remaining"));
                Application.DoEvents();
            }
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
            while (browser.Document == null && browser.Document.GetElementbyId(Element) == null && TimeOut > secondsWaited)
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
