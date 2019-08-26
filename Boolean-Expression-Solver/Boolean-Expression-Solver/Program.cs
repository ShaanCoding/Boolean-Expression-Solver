using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace Boolean_Expression_Solver
{
    class Program
    {
        //XOR NOT OR AND = ^, ', +, *
        public static void Main(string[] args)
        {
            Console.WriteLine("YOYOYO WHATS UP MIZZIA Please enter the how many variables you wish to use");

            int numberOfVariables = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Infix to Postfix");
            string userInput = Console.ReadLine();

            Console.WriteLine("[TRUTH TABLE GENERATOR]");
            bool[] valueOfVariables = new bool[numberOfVariables];
            int biggestValue = Convert.ToInt32(Math.Pow(2, numberOfVariables)) - 1;
            int biggestDigitLength = Convert.ToString(biggestValue, 2).Length;
            for (int i = 0; i < Math.Pow(2, numberOfVariables); i++)
            {
                string binary = Convert.ToString(i, 2);
                binary = binary.PadLeft(biggestDigitLength, '0');
                bool[] binaryExpression = binary.Select(c => c == '1').ToArray();
                foreach(var b in binaryExpression)
                {
                    Console.Write(b + " ");
                }
                List<string> token = Tokenize(userInput);
                List<string> RPN = GetRPN(token);
                Console.WriteLine("Output " + solver(RPN, binaryExpression));
                Console.WriteLine();
            }
        }
        public static List<string> Tokenize(string input)
        {
            List<string> TokenResult = new List<string>();
            input = Regex.Replace(input, @"\s+", "");
            string numberString = "";
            foreach (char c in input)
            {
                if (Char.IsLetter(c))
                {
                    numberString += c;
                }
                else
                {
                    TokenResult.Add(numberString);
                    numberString = "";
                    TokenResult.Add(c.ToString());
                }
            }
            return TokenResult;
        }
        static List<string> GetRPN(List<string> token)
        {
            Stack<string> s = new Stack<string>();
            List<string> PostFix = new List<string>();

            Console.Write("\nEnter your expression: ");
            try
            {
                ConvertToPostFix(token, s, ref PostFix);

                Print(PostFix);
                return PostFix;
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("Invalid expression");
                return null;
            }
        }
        static void ConvertToPostFix(List<string> symbols, Stack<string> s, ref List<string> PostFix)
        {
            int n;
            foreach (string c in symbols)
            {
                if (int.TryParse(c.ToString(), out n))
                {
                    PostFix.Add(c);
                }
                if (c == "(") s.Push(c);
                if (c == ")")
                {
                    while (s.Count != 0 && s.Peek() != "(")
                    {
                        PostFix.Add(s.Pop());
                    }
                    s.Pop();
                }
                if (IsOperator(c))
                {
                    while (s.Count != 0 && Priority(s.Peek()) >= Priority(c))
                    {
                        PostFix.Add(s.Pop());
                    }
                    s.Push(c);
                }
            }
            while (s.Count != 0)
            {
                PostFix.Add(s.Pop());
            }
        }
        static int Priority(string c)
        {
            if(c == "'")
            {
                return 3;
            }
            else if (c == "*" || c== "^")
            {
                return 2;
            }
            else if (c == "+")
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        static bool IsOperator(string c)
        {
            if (c == "*" || c == "+" || c == "^" || c == "'")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        static void Print(List<string> PostFix)
        {
            for (int i = 0; i < PostFix.Count; i++)
            {
                Console.Write("{0} ", PostFix[i]);
            }
        }
        public static bool solver(List<string> token, bool[] valuesOfVariables)
        {
            Console.WriteLine("[SOLVER]");
            Stack<bool> stack = new Stack<bool>();

            for (int i = 0; i < token.Count; i++)
            {
                if (int.TryParse(token[i], out int n))
                {
                    stack.Push(Convert.ToBoolean(valuesOfVariables[Convert.ToInt32(token[i]) - 1]));
                }
                else if (token[i] == "^" || token[i] == "*" || token[i] == "+" || token[i] == "'")
                {
                    switch (token[i])
                    {
                        case "+":
                            stack.Push(stack.Pop() | stack.Pop());
                            break;
                        case "^":
                            stack.Push(stack.Pop() ^ stack.Pop());
                            break;
                        case "*":
                            stack.Push(stack.Pop() & stack.Pop());
                            break;
                        case "'":
                            stack.Push(!stack.Pop());
                            break;
                        default:
                            throw new Exception("ERROR: Invalid Operation");
                    }
                }
            }
            return stack.Pop();
        }
    }
}
