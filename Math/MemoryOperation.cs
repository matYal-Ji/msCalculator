using System.Collections.Generic;
using System.Reflection;
using System.Resources;

namespace MathLibrary
{
    public enum Memory { Add, Subtract, Store, Recall, Clear}
    public static class MemoryOperation
    {
        private static readonly List<double> memory;
        private static readonly ResourceManager error;
        static MemoryOperation()
        {
            memory = new List<double>();
            error = new ResourceManager("MathLibrary.ExceptionMessage", Assembly.GetExecutingAssembly());
        }
        public static void Add(double result)
        {
            if(memory.Count == 0) memory.Add(result);
            else memory[memory.Count - 1] += result;
        }
        public static void Add(double result, int index)
        {
            if (index < 0 || index >= memory.Count) throw new OutOfBoundMemoryIndex(error.GetString("OutOfBound")) { Index = index };
            memory[index] += result;
        }
        public static void Subtract(double result)
        {
            if(memory.Count == 0) memory.Add(result);
            else memory[memory.Count - 1] -= result;
        }
        public static void Subtract(double result, int index)
        {
            if (index < 0 || index >= memory.Count) throw new OutOfBoundMemoryIndex(error.GetString("OutOfBound")) { Index = index };
            memory[index] -= result;
        }
        public static void Store( double result)
        {
            memory.Add(result);
        }
        public static double Recall()
        {
            return memory[memory.Count - 1];
        }
        public static double Recall(int index)
        {
            if (index < 0 || index >= memory.Count) throw new OutOfBoundMemoryIndex(error.GetString("OutOfBound")) { Index = index };
            return memory[index];
        }
        public static void Clear()
        {
            memory.Clear();
        }
        public static void Clear(int index)
        {
            if (index < 0 || index >= memory.Count) throw new OutOfBoundMemoryIndex(error.GetString("OutOfBound")) { Index = index };
            memory.RemoveAt(index);
        }
        public static List<double> ViewStack()
        {
            return memory;
        }
    }
}