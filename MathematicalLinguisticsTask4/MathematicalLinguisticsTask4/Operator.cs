using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalLinguisticsTask4
{
    public class Operator
    {
        public char Symbol { get; set; }
        public bool IsLeftAssociative { get; set; }
        public bool IsRightAssociative { get; set; }
        public int PriorityLevel { get; set; }

        public override bool Equals(object obj)
        {
            var comparedOperator = obj as Operator;
            return comparedOperator.Symbol.Equals(Symbol) &&
                comparedOperator.IsLeftAssociative.Equals(IsLeftAssociative) &&
                comparedOperator.IsRightAssociative.Equals(IsRightAssociative) &&
                comparedOperator.PriorityLevel.Equals(PriorityLevel);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
