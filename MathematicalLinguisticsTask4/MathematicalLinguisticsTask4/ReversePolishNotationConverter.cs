using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalLinguisticsTask4
{
    public class ReversePolishNotationConverter
    {
        public List<char> Digits { get; set; }
        public List<Operator> Operators { get; set; }

        private Operator LeftBracket = new Operator() { Symbol = '(' };
        private Operator RightBracket = new Operator() { Symbol = ')' };

        private Queue<string> OutputQueue = new Queue<string>();
        private Stack<Operator> OperatorsStack = new Stack<Operator>();

        public string Convert(string infixNotationInput)
        {
            infixNotationInput = infixNotationInput.Replace(" ", "");
            infixNotationInput = AddMultiplicationOnOperatorMissing(infixNotationInput);
            for (int i = 0; i < infixNotationInput.Length; i++)
            {
                var symbol = infixNotationInput[i];

                if (IsUnrecognized(symbol))
                    throw new ArgumentException($"Unrecognized symbol \"{symbol}\".");

                if (IsDigit(symbol))
                    if (i > 0 && IsDigit(infixNotationInput[i - 1]))
                        OutputQueue.Enqueue(OutputQueue.Dequeue() + symbol.ToString());
                    else
                        OutputQueue.Enqueue(symbol.ToString());
                else if (IsOperator(symbol))
                {
                    var o1 = GetOperatorFromSymbol(symbol);
                    while (OperatorsStack.Any() && !OperatorsStack.Peek().Equals(LeftBracket) &&
                        ((o1.IsLeftAssociative && o1.PriorityLevel <= OperatorsStack.Peek().PriorityLevel) ||
                           (o1.IsRightAssociative && o1.PriorityLevel < OperatorsStack.Peek().PriorityLevel)))
                    {
                        var poppedOperator = OperatorsStack.Pop();
                        OutputQueue.Enqueue(poppedOperator.Symbol.ToString());
                    }
                    OperatorsStack.Push(o1);
                }
                else if (symbol.Equals(LeftBracket.Symbol))
                    OperatorsStack.Push(LeftBracket);
                else if (symbol.Equals(RightBracket.Symbol))
                {
                    if (!OperatorsStack.Any())
                        throw new ArgumentException("Missing left bracket.");

                    while(!OperatorsStack.Peek().Equals(LeftBracket))
                    {
                        var poppedOperator = OperatorsStack.Pop();
                        OutputQueue.Enqueue(poppedOperator.Symbol.ToString());

                        if (!OperatorsStack.Any())
                            throw new ArgumentException("Missing left bracket.");
                    }
                    OperatorsStack.Pop();
                }
            }

            while(OperatorsStack.Any())
            {
                var poppedOperator = OperatorsStack.Pop();
                OutputQueue.Enqueue(poppedOperator.Symbol.ToString());
            }

            //if (OutputQueue.Contains("("))
            //    throw new ArgumentException("Incorrect brackets.");

            StringBuilder resultBuilder = new StringBuilder();
            while (OutputQueue.Any())
            {
                resultBuilder.Append(OutputQueue.Dequeue());
                resultBuilder.Append(' ');
            }

            return resultBuilder.ToString();
        }

        private string AddMultiplicationOnOperatorMissing(string infixNotationInput)
        {
            var result = infixNotationInput;
            int i = 1;
            int j = 0;
            while(i<infixNotationInput.Length-1)
            { 
                var symbol = infixNotationInput[i];
                if (symbol.Equals('(') && IsDigit(infixNotationInput[i - 1]))
                {
                    result = result.Insert(i+j, "*");
                    j++;
                }
                else if (symbol.Equals(')') && IsDigit(infixNotationInput[i + 1]))
                {
                    result = result.Insert(i + j+1, "*");
                    j++;
                }
                i++;
            }
            return result;
        }

        private bool IsDigit(char symbol)
            => Digits.Contains(symbol);

        private bool IsOperator(char symbol)
            => Operators.Select(o => o.Symbol).Contains(symbol);

        private Operator GetOperatorFromSymbol(char symbol)
            => Operators.SingleOrDefault(o => o.Symbol.Equals(symbol));

        private bool IsUnrecognized(char symbol)
            => !IsDigit(symbol) &&
            !IsOperator(symbol) &&
            !LeftBracket.Symbol.Equals(symbol) &&
            !RightBracket.Symbol.Equals(symbol);
    }
}
