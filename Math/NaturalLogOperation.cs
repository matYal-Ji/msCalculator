using System;

namespace MathLibrary
{
    public class NaturalLogOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return Math.Log(operands[0]);
        }
    }
}