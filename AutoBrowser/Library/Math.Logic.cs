using System.Collections.Generic;

namespace AutoBrowser.Library
{
    public static partial class Math
    {
        public static bool CalculateLogic(string expression)
        {
            var postFija = ExpressionToPostList(expression);

            return ResolveLogicOperations(postFija);
        }

        private static bool ResolveLogicOperations(List<string> postFija)
        {
            Stack<object> values = new Stack<object>();
            bool result = false;
            foreach (string item in postFija)
            {
                if (_operators.Find(x => x.Value.Equals(item)) == null)
                {
                    values.Push(item);
                }
                else
                {
                    object a, b;
                    a = values.Pop();
                    b = values.Pop();
                    result = ResolveLogicOperation(a, item.Trim(), b);
                    values.Push(result);
                }
            }

            return result;
        }

        private static bool ResolveLogicOperation(object val1, string ope, object val2)
        {
            switch (ope)
            {
                case "==":
                    if (val1 is int v1 && val2 is int v2)
                    {
                        return v1 == v2;
                    }
                    else if (val1 is string s1 && val2 is string s2)
                    {
                        return s1 == s2;
                    }
                    else if (val1 is bool b1 && val2 is bool b2)
                    {
                        return b1 == b2;
                    }
                    else
                    {
                        return false;
                    }
                case "!=":
                    if (val1 is int va1 && val2 is int va2)
                    {
                        return va1 != va2;
                    }
                    else if (val1 is string s1 && val2 is string s2)
                    {
                        return s1 != s2;
                    }
                    else if (val1 is bool b1 && val2 is bool b2)
                    {
                        return b1 != b2;
                    }
                    else
                    {
                        return false;
                    }
                case ">":
                    if (val1 is int _v1 && val2 is int _v2)
                    {
                        return _v1 > _v2;
                    }
                    else
                    {
                        return false;
                    }
                case "<":
                    if (val1 is int var1 && val2 is int var2)
                    {
                        return var1 < var2;
                    }
                    else
                    {
                        return false;
                    };
                default: return false;
            }
        }
    }
}
