namespace MathLibrary
{
    public class InvalidExpressionException : MathLibraryException
    {
        public InvalidExpressionException(string message) : base(message)
        {
        }
    }
}