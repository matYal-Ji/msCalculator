using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLibrary
{
    internal class InvalidMemoryOperationException : MathLibraryException
    {
        public Memory OperationType { get; set; }
        public InvalidMemoryOperationException(string message) : base(message)
        {
        }
    }
}
