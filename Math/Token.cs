namespace MathLibrary
{
    public enum TokenType { Operand, Operator };
    public class Token
    {
        public object Value { get; set; }
        public TokenType TokenType { get; set; }
    }
}