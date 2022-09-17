namespace MathLibrary
{
    public class NumberOfOperandInOperationException : MathLibraryException
    {
        public int Actual { get; set; }
        public int Expected { get; set; }
        public NumberOfOperandInOperationException(string message) : base(message)
        {
        }
    }
}