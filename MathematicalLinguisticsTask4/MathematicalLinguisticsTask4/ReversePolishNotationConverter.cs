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

        private Queue<char> OutputQueue = new Queue<char>();
        private Stack<Operator> OperatorsStack = new Stack<Operator>();

        public string Convert(string infixNotationInput)
        {
            foreach (char symbol in infixNotationInput)
            {
                if (IsUnrecognized(symbol))
                    throw new ArgumentException($"Unrecognized symbol \"{symbol}\".");

                if (IsDigit(symbol))
                    OutputQueue.Enqueue(symbol);
                else if (IsOperator(symbol))
                {
                    var o1 = GetOperatorFromSymbol(symbol);
                    while (OperatorsStack.Any() && !OperatorsStack.Peek().Equals(LeftBracket) &&
                        ((o1.IsLeftAssociative && o1.PriorityLevel <= OperatorsStack.Peek().PriorityLevel) ||
                           (o1.IsRightAssociative && o1.PriorityLevel < OperatorsStack.Peek().PriorityLevel)))
                    {
                        var poppedOperator = OperatorsStack.Pop();
                        OutputQueue.Enqueue(poppedOperator.Symbol);
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
                        OutputQueue.Enqueue(poppedOperator.Symbol);

                        if (!OperatorsStack.Any())
                            throw new ArgumentException("Missing left bracket.");
                    }
                    OperatorsStack.Pop();
                }

            }

            StringBuilder resultBuilder = new StringBuilder();
            while (OutputQueue.Any())
                resultBuilder.Append(OutputQueue.Dequeue());

            return resultBuilder.ToString();
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
