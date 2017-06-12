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

            var negativeValueEncountered = false;
            var tempNumber = new StringBuilder();

            for (int i = 0; i < infixNotationInput.Length; i++)
            {
                var symbol = infixNotationInput[i];

                if (IsUnrecognized(symbol))
                    throw new ArgumentException($"Unrecognized symbol \"{symbol}\".");

                if (negativeValueEncountered)
                {
                    if (IsDigit(symbol))
                    {
                        tempNumber.Append("-");
                        tempNumber.Append(symbol);
                        negativeValueEncountered = false;
                    }
                    else
                        throw new ArgumentException("Minus sign has invalid symbol on it's right-hand side.");
                }
                else
                {

                    if (IsDigit(symbol))
                    {
                        tempNumber.Append(symbol);
                        if (i > infixNotationInput.Length - 2 || !IsDigit(infixNotationInput[i + 1]))
                        {
                            OutputQueue.Enqueue(tempNumber.ToString());
                            tempNumber.Clear();
                        }
                    }
                    else if (IsOperator(symbol))
                    {
                        var o1 = GetOperatorFromSymbol(symbol);

                        if (o1.Symbol.Equals('-') && (i == 0 || !IsDigit(infixNotationInput[i - 1])))
                            negativeValueEncountered = true;
                        else
                        {
                            while (OperatorsStack.Any() && !OperatorsStack.Peek().Equals(LeftBracket) &&
                                ((o1.IsLeftAssociative && o1.PriorityLevel <= OperatorsStack.Peek().PriorityLevel) ||
                                (o1.IsRightAssociative && o1.PriorityLevel < OperatorsStack.Peek().PriorityLevel)))
                            {
                                var poppedOperator = OperatorsStack.Pop();
                                OutputQueue.Enqueue(poppedOperator.Symbol.ToString());
                            }
                            OperatorsStack.Push(o1);
                        }
                    }
                    else if (symbol.Equals(LeftBracket.Symbol))
                        OperatorsStack.Push(LeftBracket);
                    else if (symbol.Equals(RightBracket.Symbol))
                    {
                        if (!OperatorsStack.Any())
                            throw new ArgumentException("Missing left bracket.");

                        while (!OperatorsStack.Peek().Equals(LeftBracket))
                        {
                            var poppedOperator = OperatorsStack.Pop();
                            OutputQueue.Enqueue(poppedOperator.Symbol.ToString());

                            if (!OperatorsStack.Any())
                                throw new ArgumentException("Missing left bracket.");
                        }
                        OperatorsStack.Pop();
                    }
                }
            }

            while(OperatorsStack.Any())
            {
                var poppedOperator = OperatorsStack.Pop();
                OutputQueue.Enqueue(poppedOperator.Symbol.ToString());
            }

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
