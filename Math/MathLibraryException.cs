using System;

namespace MathLibrary
{
    public class MathLibraryException : Exception
    {
        override public string Message { get; }
        public MathLibraryException(string message)
        {
            Message = message;
        }
    }
}