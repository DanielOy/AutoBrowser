using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharedLibrary
{
    public static partial class Math
    {
        #region Global Variables
        private static readonly List<Operator> _operators = new List<Operator>()
        {
            new Operator("+",1,true),
            new Operator("-",1,true),
            new Operator("*",2,true),
            new Operator("/",2,true),
            new Operator("(",3,false),
            new Operator(")",3,false),
            new Operator("%",0,false),
            new Operator("==",0,false),
            new Operator("!=",0,false),
            new Operator(">",0,false),
            new Operator("<",0,false),
            new Operator(">=",0,false),
            new Operator("<=",0,false),
        };
        #endregion

        private static List<string> ExpressionToPostList(string expression)
        {
            if (expression.StartsWith("-"))
            {
                expression = "0" + expression;
            }

            List<string> values = Regex.Split(expression, @"(\+)|(\-)|(\*)|(\/)|(\()|(\))|(\%)|(==)|(<(?!=))|(>(?!=))|(<=)|(>=)").ToList();
            values.RemoveAll(x => string.IsNullOrEmpty(x?.Trim()));

            return InfijaToPostFija(values);
        }

        private static List<string> InfijaToPostFija(List<string> values)
        {
            Stack<string> valuesStack = new Stack<string>();
            List<string> valuesPost = new List<string>();

            string topValue;
            int priorityItemStack, priorityItem;

            foreach (string value in values)
            {

                if (value == "(")
                {
                    valuesStack.Push(value);
                }
                else if (value == ")")
                {
                    topValue = valuesStack.Pop();
                    while (!(topValue == "("))
                    {
                        valuesPost.Add(topValue);
                        topValue = valuesStack.Pop();
                    }
                }
                else if (_operators.Find(x => x.Value.Equals(value)) == null)
                {
                    valuesPost.Add(value);
                }
                else
                {
                    priorityItem = _operators.Find(x => x.Value.Equals(value))?.Priority ?? 0;
                    priorityItemStack = 0;
                    topValue = "";

                    if (valuesStack.Count > 0)
                    {
                        topValue = valuesStack.Peek();
                        priorityItemStack = _operators.Find(x => x.Value.Equals(topValue))?.Priority ?? 0;
                    }

                    while (valuesStack.Count > 0 && priorityItem <= priorityItemStack)
                    {
                        valuesPost.Add(valuesStack.Pop());
                        priorityItem = _operators.Find(x => x.Value.Equals(value))?.Priority ?? 0;

                        if (valuesStack.Count > 0)
                        {
                            topValue = valuesStack.Peek();
                        }

                        priorityItemStack = _operators.Find(x => x.Value.Equals(topValue))?.Priority ?? 0;
                    }

                    valuesStack.Push(value);
                }
            }

            while (valuesStack.Count > 0)
            {
                valuesPost.Add(valuesStack.Pop());
            }

            return valuesPost;
        }

        private class Operator
        {
            public Operator(string value, int priority, bool isAritmetic)
            {
                Value = value;
                Priority = priority;
                IsAritmetic = isAritmetic;
            }

            public string Value { get; set; }
            public int Priority { get; set; }
            public bool IsAritmetic { get; set; }
        }
    }
}
