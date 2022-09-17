using System.Reflection;
using System.Resources;

namespace MathLibrary
{
    public abstract class BinaryOperation : IOperation
    {
        protected ResourceManager error = new ResourceManager("MathLibrary.ExceptionMessage", Assembly.GetExecutingAssembly());
        public int OperandCount { get; }

        public BinaryOperation()
        {
            OperandCount = 2;
        }

        public double Calculate(double[] operands)
        {
            if (operands.Length != OperandCount)
            {
                string message;
                if (operands.Length < OperandCount)
                {
                    message = error.GetString("TooLessOperand");
                }
                else
                {
                    message = error.GetString("TooManyOperand");
                }
                NumberOfOperandInOperationException exception = new NumberOfOperandInOperationException(message)
                {
                    Actual = operands.Length,
                    Expected = OperandCount
                };
                throw exception;
            }
            return Evaluate(operands);
        }
        protected abstract double Evaluate(double[] operands);
    }
}