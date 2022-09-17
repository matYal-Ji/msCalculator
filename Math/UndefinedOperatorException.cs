namespace MathLibrary
{
    public class UndefinedOperatorException : MathLibraryException
    {
        public string Operator { get; set; }
        public UndefinedOperatorException(string message) : base(message)
        {
        }
    }
}