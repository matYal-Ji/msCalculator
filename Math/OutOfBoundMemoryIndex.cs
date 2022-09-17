namespace MathLibrary
{
    internal class OutOfBoundMemoryIndex : MathLibraryException
    {
        public int Index { get; set; }
        public OutOfBoundMemoryIndex(string message) : base(message)
        {
        }
    }
}
