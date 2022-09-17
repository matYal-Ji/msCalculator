namespace MathLibrary
{
    public interface IOperation
    {
        int OperandCount { get; }
        double Calculate(double[] operands);
    }
}