using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AutoBrowser.Core.Actions
{
    public class Conditional : BaseAction
    {
        #region Global Variables
        private string _originalExpression;
        private string _originalParam1;
        private string _originalParam2;
        #endregion

        #region Properties
        public string Expression { get; set; }

        public List<BaseAction> Actions { get; set; }
        public ConditionalFunctions SpecialFunction { get; set; } = ConditionalFunctions.None;

        public enum ConditionalFunctions
        {
            None = 0,
            _FileContains_ = 1,
            _FileExists = 2,
            _FileExistsIn_Folder = 3,
            _FileNotContains_ = 4
        }
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        #endregion

        #region Constructors
        public Conditional()
        { }

        public Conditional(List<BaseAction> actions, string expression)
        {
            Expression = _originalExpression = expression;
            Actions = actions;
        }
        public Conditional(List<BaseAction> actions, ConditionalFunctions function, string param1, string param2 = "")
        {
            Expression = _originalExpression = "";
            SpecialFunction = function;
            Actions = actions;
            Param1 = _originalParam1 = param1;
            Param2 = _originalParam2 = param2;
        }
        #endregion

        #region Functions
        protected override void ResetValues()
        {
            Expression = _originalExpression;
            Param1 = _originalParam1;
            Param2 = _originalParam2;
        }

        internal override void InitVariables()
        {
            _originalExpression = Expression;
            _originalParam1 = Param1;
            _originalParam2 = Param2;
            Actions?.ForEach(x => x.InitVariables());
        }

        public bool EvaluateCondition()
        {
            if (SpecialFunction == ConditionalFunctions.None)
            {
                Expression = Expression.Replace("-", "_");
                Expression = Expression.Replace(" ", "");
                return SharedLibrary.Math.CalculateLogic(Expression);
            }
            else
            {
                return ExecuteSpecialFunction();
            }
        }

        private bool ExecuteSpecialFunction()
        {
            switch (SpecialFunction)
            {
                case ConditionalFunctions._FileContains_: return FileContainsFunc();
                case ConditionalFunctions._FileNotContains_: return !FileContainsFunc();
                case ConditionalFunctions._FileExists: return FileExistsFunc();
                case ConditionalFunctions._FileExistsIn_Folder: return FileExistsFolderFunc();
                default: return false;
            }
        }

        private bool FileExistsFolderFunc()
        {
            if (string.IsNullOrEmpty(Param1) || string.IsNullOrEmpty(Param2))
            {
                return false;
            }

            if (!Directory.Exists(Param2))
            {
                return false;
            }

            return Directory.GetFiles(Param2).Contains($"{Param2}\\{Param1}");
        }

        private bool FileExistsFunc()
        {
            if (string.IsNullOrEmpty(Param1))
            {
                return false;
            }

            return File.Exists(Param1);
        }

        private bool FileContainsFunc()
        {
            if (string.IsNullOrEmpty(Param1) || string.IsNullOrEmpty(Param2))
            {
                return false;
            }

            if (!FileExistsFunc())
            {
                return false;
            }

            return File.ReadAllLines(Param1).Contains(Param2);
        }

        public override string GetDescription()
        {
            if (string.IsNullOrEmpty(Expression))
            {
                return $"If <{Param1}> <{SpecialFunction.ToString()}> <{Param2}>";
            }
            else
            {
                return $"If <{Expression}>";
            }
        }
        #endregion
    }
}
