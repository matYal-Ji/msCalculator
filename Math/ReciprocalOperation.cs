using System;

namespace MathLibrary
{
    public class ReciprocalOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            if (operands[0] == 0) throw new DivideByZeroException(error.GetString("DivideByZero"));
            return 1 / operands[0];
        }
    }
}