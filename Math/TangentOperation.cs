using System;

namespace MathLibrary
{
    public class TangentOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return Math.Tan(operands[0]);
        }
    }
}