using System;

namespace MathLibrary
{
    public class CotangentOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return 1/(Math.Tan(operands[0]));
        }
    }
}