namespace MathLibrary
{
    public class NegativeOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return -operands[0];
        }
    }
}