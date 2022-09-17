using System;

namespace MathLibrary
{
    public class CosineOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return Math.Cos(operands[0]);
        }
    }
}