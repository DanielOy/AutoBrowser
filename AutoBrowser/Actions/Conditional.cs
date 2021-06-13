using System.Collections.Generic;

namespace AutoBrowser.Actions
{
    public class Conditional : BaseAction
    {
        #region Global Variables
        private string _originalExpression;
        #endregion

        #region Properties
        public string Expression { get; set; }

        public List<BaseAction> Actions { get; set; }
        #endregion

        #region Constructors
        public Conditional()
        { }

        public Conditional(string expression, List<BaseAction> actions)
        {
            Expression = _originalExpression = expression;
            Actions = actions;
        }
        #endregion

        #region Functions
        protected override void ResetValues()
        {
            Expression = _originalExpression;
        }

        internal override void InitVariables()
        {
            _originalExpression = Expression;
            Actions?.ForEach(x => x.InitVariables());
        }

        public bool EvaluateCondition()
        {
            Expression = Expression.Replace("-", "_");
            Expression = Expression.Replace(" ", "");
            return Library.Math.CalculateLogic(Expression);
        }
        #endregion
    }
}
