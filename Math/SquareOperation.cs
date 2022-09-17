namespace MathLibrary
{
    public class SquareOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return operands[0] * operands[0];
        }
    }
}