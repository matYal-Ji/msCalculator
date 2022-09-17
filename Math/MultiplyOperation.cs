namespace MathLibrary
{
    public class MultiplyOperation : BinaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return operands[0] * operands[1];
        }
    }
}