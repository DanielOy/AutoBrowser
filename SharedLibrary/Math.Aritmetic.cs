using System;
using System.Collections.Generic;

namespace SharedLibrary
{
    public static partial class Math
    {
        public static int Calculate(string operation)
        {
            var postFija = ExpressionToPostList(operation);

            return ResolvePostOperations(postFija);
        }

        private static int ResolvePostOperations(List<string> postFija)
        {
            if (postFija.Count==1)
            {
                return Convert.ToInt32(postFija[0]);
            }

            Stack<int> numbers = new Stack<int>();
            int total = 0;
            foreach (string item in postFija)
            {
                if (int.TryParse(item, out int number))
                {
                    numbers.Push(number);
                }
                else if (item != ">=" && item != "<=")
                {
                    int b = numbers.Pop();
                    int a = numbers.Pop();
                    total = ResolveOperation(a, item.Trim(), b);
                    numbers.Push(total);
                }
            }

            return total;
        }

        private static int ResolveOperation(int a, string item, int b)
        {
            switch (item)
            {
                case "+": return a + b;
                case "-": return a - b;
                case "*": return a * b;
                case "/": return a / b;
                default: return 0;
            }
        }
    }
}
