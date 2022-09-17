namespace MathLibrary
{
    public class AddOperation : BinaryOperation
    {
        protected override double Evaluate(double[] operands)
        {
            return operands[0] + operands[1];
        }
    }
}