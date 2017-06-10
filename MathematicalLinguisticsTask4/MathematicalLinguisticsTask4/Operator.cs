using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathematicalLinguisticsTask4
{
    public class Operator
    {
        public char Value { get; set; }
        public bool IsLeftAssociative { get; set; }
        public bool IsRightAssociative { get; set; }
        public int PriorityLevel { get; set; }
    }
}
