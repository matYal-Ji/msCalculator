using System;

namespace MathLibrary
{
    public class CosecantOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            double result = Math.Sin(operands[0]);
            if (result == 0) throw new DivideByZeroException(error.GetString("DivideByZero"));
            return 1 / result;
        }
    }
}
