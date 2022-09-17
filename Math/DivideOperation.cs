using System;

namespace MathLibrary
{
    public class DivideOperation : BinaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            if (operands[1] == 0) throw new DivideByZeroException(error.GetString("DivideByZero"));
            return operands[0] / operands[1];
        }
    }
}