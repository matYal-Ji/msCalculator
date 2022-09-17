using System;

namespace MathLibrary
{
    public class LogOperation : BinaryOperation
    {
        protected override double Evaluate(double[] operands)
        { 
            return Math.Log(operands[1], operands[0]);
        }
    }
}