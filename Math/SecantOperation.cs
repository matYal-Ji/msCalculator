using System;

namespace MathLibrary
{
    public class SecantOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            double result = Math.Cos(operands[0]);
            if(result == 0)
            {
                throw new DivideByZeroException(error.GetString("DivideByZero"));
            }
            return result;
        }
    }
}