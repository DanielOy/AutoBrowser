using System;
using System.Collections.Generic;

namespace AutoBrowser.Actions
{
    public class Conditional : BaseAction
    {
        private readonly string _originalExpression;

        public Conditional(string expression, List<BaseAction> actions)
        {
            Expression = _originalExpression = expression;
            Actions = actions;
        }

        public string Expression { get; set; }

        public List<BaseAction> Actions { get; set; }

        protected override void ResetValues()
        {
            Expression = _originalExpression;
        }

        public bool EvaluateCondition()
        {
            Expression = Expression.Replace("-","_");
            Expression = Expression.Replace(" ","");
            return Library.Math.CalculateLogic(Expression);
        }
    }
}
