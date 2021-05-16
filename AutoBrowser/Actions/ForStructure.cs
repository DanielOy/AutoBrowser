using System.Collections.Generic;

namespace AutoBrowser.Actions
{
    public class ForStructure : BaseAction
    {
        private readonly string _originalName;
        private readonly object _originalTimes;
        private readonly string _originalIndexFormula;

        public string Name { get; set; }
        public List<BaseAction> Actions { get; set; }
        public object Times { get; set; }
        public string IndexFormula { get; set; }

        public ForStructure(string name, object times, List<BaseAction> actions)
        {
            Name = _originalName = name;
            Times = _originalTimes = times;
            Actions = actions;
            IndexFormula = "";
        }

        public ForStructure(string name, object times, string indexFormula, List<BaseAction> actions)
        {
            Name = _originalName = name;
            Times = _originalTimes = times;
            IndexFormula = _originalIndexFormula = indexFormula;
            Actions = actions;
        }

        public override void ReplaceVariables(Dictionary<string, object> savedValues)
        {
            if (savedValues == null)
            {
                return;
            }

            ResetValues();

            foreach (var item in savedValues)
            {
                if (Name.Contains($"[{item.Key}]"))
                {
                    Name = _originalName.Replace($"[{item.Key}]", item.Value.ToString());
                }

                if (Times.ToString().Contains($"[{item.Key}]"))
                {
                    Times = _originalTimes.ToString().Replace($"[{item.Key}]", item.Value.ToString());
                }

                if (IndexFormula.ToString().Contains($"[{item.Key}]"))
                {
                    IndexFormula = _originalIndexFormula.ToString().Replace($"[{item.Key}]", item.Value.ToString());
                }
            }
        }

        protected override void ResetValues()
        {
            Name = _originalName;
            Times = _originalTimes;
            IndexFormula = _originalIndexFormula;
        }

        public int Calculate(int i)
        {
            if (string.IsNullOrEmpty(IndexFormula))
            {
                return i;
            }

            return Classes.MathOperations.Calculate(IndexFormula.Replace($"[{Name}]", i.ToString()));
        }
    }
}
