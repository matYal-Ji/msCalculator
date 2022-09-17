using System;

namespace MathLibrary
{
    public class SquareRootOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return Math.Sqrt(operands[0]);
        }
    }
}