using System.Collections.Generic;

namespace AutoBrowser.Actions
{
    public class Repeat : BaseAction
    {
        #region Variables
        private readonly string _originalName;
        private readonly object _originalTimes;
        private readonly string _originalIndexFormula;
        #endregion

        #region Properties
        public string Name { get; set; }
        public List<BaseAction> Actions { get; set; }
        public object Times { get; set; }
        public string IndexFormula { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new object that contains a list of actions to repeat in base the number of times.
        /// </summary>
        /// <param name="name">Name for the current object, this can be used as an index. That index start with 0 by default, unless it change with a formula.</param>
        /// <param name="times">A number o the formula to calculate the number of repetitions.</param>
        /// <param name="actions">A list of actions to do.</param>
        public Repeat(string name, object times, List<BaseAction> actions)
        {
            Name = _originalName = name;
            Times = _originalTimes = times;
            Actions = actions;
            IndexFormula = _originalIndexFormula = "";
        }

        /// <summary>
        /// Create a new object that contains a list of actions to repeat in base the number of times.
        /// </summary>
        /// <param name="name">Name for the current object, this can be used as an index. That index start with 0 by default, unless it change with a formula.</param>
        /// <param name="times">A number o the formula to calculate the number of repetitions.</param>
        /// <param name="indexFormula">Define a formula to apply to the index, that value can by used with the given name.</param>
        /// <param name="actions">A list of actions to do.</param>
        public Repeat(string name, object times, string indexFormula, List<BaseAction> actions)
        {
            Name = _originalName = name;
            Times = _originalTimes = times;
            IndexFormula = _originalIndexFormula = indexFormula;
            Actions = actions;
        }
        #endregion

        #region Functions
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

            return Library.Math.Calculate(IndexFormula.Replace($"[{Name}]", i.ToString()));
        } 
        #endregion
    }
}
