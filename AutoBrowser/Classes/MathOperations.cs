using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AutoBrowser.Classes
{
    public static class MathOperations
    {
        //TODO: Improve the code.
        public static int Calculate(string operation)
        {
            if (operation.StartsWith("-"))
            {
                operation = "0" + operation;
            }

            List<string> values = Regex.Split(operation, @"(\+)|(\-)|(\*)|(\/)|(\()|(\))|(\%)|(==)|(<(?!=))|(>(?!=))|(<=)|(>=)").ToList();
            values.RemoveAll(x => string.IsNullOrEmpty(x?.Trim()));

            List<string> postFija = InfijaToPostFija(values);

            return ResolveOperations(postFija);
        }

        private static int ResolveOperations(List<string> postFija)
        {
            Stack<int> numbers = new Stack<int>();
            int total = 0;
            foreach (string item in postFija)
            {
                if (!int.TryParse(item, out int number))
                {
                    if (item == ">=" || item == "<=")
                    {
                        //int a, b;
                        //a = numbers.Pop();
                        //b = numbers.Pop();
                        //numbers.Push("T" + contadorT);

                    }
                    else
                    {
                        int a, b;
                        a = numbers.Pop();
                        b = numbers.Pop();
                        total = ResolveOperation(a, item.Trim(), b);
                        numbers.Push(total);
                    }
                }
                else
                {
                    numbers.Push(number);
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

        private static List<string> InfijaToPostFija(List<string> values)
        {
            Stack<string> pila = new Stack<string>();
            string valorTope;

            List<string> postFija = new List<string>();
            string cadenaPost = "";

            foreach (string item in values)
            {

                if (item == "(")
                {
                    pila.Push(item);
                }
                else if (item == ")")
                {

                    valorTope = pila.Pop();
                    while (!(valorTope == "("))
                    {
                        cadenaPost = $"{cadenaPost} {valorTope}";
                        postFija.Add(valorTope);
                        valorTope = pila.Pop();
                    }
                }
                else if (!(item == "+" || item == "-" || item == "*" || item == "/" || item == "%" || item == "==" || item == ">" || item == "<" || item == ">=" || item == "<="))
                {
                    cadenaPost = cadenaPost + " " + item;
                    postFija.Add(item);
                }
                else if (item == "+" || item == "-" || item == "*" || item == "/" || item == "%" || item == "==" || item == ">" || item == "<" || item == ">=" || item == "<=")
                {
                    int prioridadItem = 0;
                    int prioridadPila = 0;
                    string aux = "";
                    if (item == "+")
                    {
                        prioridadItem = 1;
                    }

                    if (item == "-")
                    {
                        prioridadItem = 1;
                    }

                    if (item == "/")
                    {
                        prioridadItem = 2;
                    }

                    if (item == "*")
                    {
                        prioridadItem = 2;
                    }

                    if (item == ")")
                    {
                        prioridadItem = 3;
                    }

                    if (item == "(")
                    {
                        prioridadItem = 3;
                    }

                    if (item == "%")
                    {
                        prioridadItem = 0;
                    }

                    if (item == "==")
                    {
                        prioridadItem = 0;
                    }

                    if (item == "<")
                    {
                        prioridadItem = 0;
                    }

                    if (item == ">")
                    {
                        prioridadItem = 0;
                    }

                    if (item == "<=")
                    {
                        prioridadItem = 0;
                    }

                    if (item == ">=")
                    {
                        prioridadItem = 0;
                    }

                    if (pila.Count > 0) { aux = pila.Pop(); pila.Push(aux); }
                    if (aux == "+")
                    {
                        prioridadPila = 1;
                    }

                    if (aux == "-")
                    {
                        prioridadPila = 1;
                    }

                    if (aux == "*")
                    {
                        prioridadPila = 2;
                    }

                    if (aux == "/")
                    {
                        prioridadPila = 2;
                    }

                    if (aux == "%")
                    {
                        prioridadPila = 0;
                    }

                    if (aux == "==")
                    {
                        prioridadPila = 0;
                    }

                    if (aux == "<")
                    {
                        prioridadPila = 0;
                    }

                    if (aux == ">")
                    {
                        prioridadPila = 0;
                    }

                    if (aux == "<=")
                    {
                        prioridadPila = 0;
                    }

                    if (aux == ">=")
                    {
                        prioridadPila = 0;
                    }

                    if (aux == ")")
                    {
                        prioridadItem = 3;
                    }

                    if (aux == "(")
                    {
                        prioridadItem = 3;
                    }

                    while (pila.Count > 0 && prioridadItem <= prioridadPila)
                    {
                        string b = pila.Pop();
                        cadenaPost = cadenaPost + " " + b;
                        postFija.Add(b);
                        if (item == "==")
                        {
                            prioridadItem = 0;
                        }

                        if (item == "%")
                        {
                            prioridadItem = 0;
                        }

                        if (item == "+")
                        {
                            prioridadItem = 1;
                        }

                        if (item == "-")
                        {
                            prioridadItem = 1;
                        }

                        if (item == "/")
                        {
                            prioridadItem = 2;
                        }

                        if (item == "*")
                        {
                            prioridadItem = 2;
                        }

                        if (item == ")")
                        {
                            prioridadItem = 3;
                        }

                        if (item == "(")
                        {
                            prioridadItem = 3;
                        }

                        if (item == "<")
                        {
                            prioridadItem = 0;
                        }

                        if (item == ">")
                        {
                            prioridadItem = 0;
                        }

                        if (item == "<=")
                        {
                            prioridadItem = 0;
                        }

                        if (item == ">=")
                        {
                            prioridadItem = 0;
                        }

                        if (pila.Count > 0) { aux = pila.Pop(); pila.Push(aux); }
                        if (aux == "%")
                        {
                            prioridadPila = 0;
                        }

                        if (aux == "==")
                        {
                            prioridadPila = 0;
                        }

                        if (aux == "<")
                        {
                            prioridadPila = 0;
                        }

                        if (aux == ">")
                        {
                            prioridadPila = 0;
                        }

                        if (aux == "<=")
                        {
                            prioridadPila = 0;
                        }

                        if (aux == ">=")
                        {
                            prioridadPila = 0;
                        }

                        if (aux == "+")
                        {
                            prioridadPila = 1;
                        }

                        if (aux == "-")
                        {
                            prioridadPila = 1;
                        }

                        if (aux == "*")
                        {
                            prioridadPila = 2;
                        }

                        if (aux == "/")
                        {
                            prioridadPila = 2;
                        }

                        if (aux == ")")
                        {
                            prioridadItem = 3;
                        }

                        if (aux == "(")
                        {
                            prioridadItem = 3;
                        }
                    }
                    pila.Push(item);
                }
            }

            while (pila.Count > 0)
            {
                string b = pila.Pop();
                cadenaPost = cadenaPost + " " + b;
                postFija.Add(b);
            }

            return postFija;
        }
    }
}
