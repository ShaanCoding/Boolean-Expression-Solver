using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Text.RegularExpressions;

namespace BooleanExpressionSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberOfTerms_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UserInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            DataTable dt = new DataTable();

            if (!string.IsNullOrEmpty(UserInput.Text))
            {
                int numberOfVariables = Convert.ToInt32(numberOfTerms.Text);
                string userInput = UserInput.Text;

                int biggestValue = Convert.ToInt32(Math.Pow(2, numberOfVariables)) - 1;
                int biggestDigitLength = Convert.ToString(biggestValue, 2).Length;

                DataColumn output = new DataColumn("Output", typeof(string));

                for (int i = 1; i <= numberOfVariables; i++)
                {
                    dt.Columns.Add(new DataColumn(i.ToString(), typeof(string)));
                }
                dt.Columns.Add(output);

                for (int i = 0; i < Math.Pow(2, numberOfVariables); i++)
                {
                    string binary = Convert.ToString(i, 2);
                    binary = binary.PadLeft(biggestDigitLength, '0');
                    bool[] binaryExpression = binary.Select(c => c == '1').ToArray();

                    DataRow inputRow = dt.NewRow();

                    //Add
                    for (int j = 0; j < binaryExpression.Length; j++)
                    {
                        if (binaryExpression[j] == true)
                        {
                            inputRow[j] = "True";
                        }
                        else if (binaryExpression[j] == false)
                        {
                            inputRow[j] = "False";
                        }
                    }

                    List<string> token = Tokenize(userInput);
                    List<string> RPN = GetRPN(token);
                    
                    bool truthTableValue = solver(RPN, binaryExpression);
                    if (truthTableValue == true)
                    {
                        inputRow[binaryExpression.Length] = "True";
                    }
                    else
                    {
                        inputRow[binaryExpression.Length] = "False";
                    }
                    dt.Rows.Add(inputRow);
                }

                TruthTable.ItemsSource = dt.DefaultView;

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

            try
            {
                ConvertToPostFix(token, s, ref PostFix);
                return PostFix;
            }
            catch (InvalidOperationException)
            {
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
            if (c == "'")
            {
                return 3;
            }
            else if (c == "*" || c == "^")
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

        private void TruthTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
