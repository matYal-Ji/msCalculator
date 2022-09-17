using System;

namespace MathLibrary
{
    public class PowerOperation : BinaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return Math.Pow(operands[0], operands[1]);
        }
    }
}