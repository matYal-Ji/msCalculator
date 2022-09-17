namespace MathLibrary
{
    public class AbsoluteOperation : UnaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            if(operands[0] < 0) return -operands[0];
            return operands[0];
        }
    }
}