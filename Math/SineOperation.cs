using System;

namespace MathLibrary
{
    public class SineOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return Math.Sin(operands[0]);
        }
    }
}