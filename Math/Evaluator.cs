using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace MathLibrary
{
    public class Evaluator
    {
        static protected readonly Dictionary<string, IOperation> operations;
        static protected readonly List<Operator> operators;
        static protected readonly ResourceManager error;
        static Evaluator()
        {
            operations = new Dictionary<string, IOperation>();
            operators = ParseExpression.GetOperatorTable();
            error = new ResourceManager("MathLibrary.ExceptionMessage", Assembly.GetExecutingAssembly());
        }
        public double Evaluate(Memory operation)
        {
            if (operation == Memory.Add || operation == Memory.Subtract || operation == Memory.Store)
            {
                throw new InvalidMemoryOperationException(error.GetString("ExpressionNotFound")) { OperationType = operation };
            }
            switch (operation)
            {
                case Memory.Recall:
                    return MemoryOperation.Recall();
                case Memory.Clear:
                    MemoryOperation.Clear();
                    break;
            }
            return 0;
        }
        public double Evaluate(string expression)
        {
            bool lessOperand = false, validOperation = true;
            int actualOperands = 0, expectedOperands = 0;
            Stack<Token> operandStack = new Stack<Token>();
            List<Token> postfixExpression = ParseExpression.ToPostfix(expression, operators);
            foreach (Token token in postfixExpression)
            {
                if (token.TokenType == TokenType.Operand)
                {
                    operandStack.Push(token);
                    actualOperands++;
                }
                else
                {
                    if (operations.TryGetValue(operators.Where(a => a.Symbol == (string)token.Value).First().Class, out IOperation operatorClass))
                    {

                    }
                    else
                    {
                        operatorClass = (IOperation)Activator.CreateInstance(Type.GetType(operators.Where(a => a.Symbol == (string)token.Value).First().Class));   //To get type of class using its name
                        operations[operators.Where(a => a.Symbol == (string)token.Value).First().Class] = operatorClass;
                    }
                    int operandCount = operatorClass.OperandCount;     //Get Interface Property through reflection
                    expectedOperands += operandCount;
                    if (validOperation)
                    {
                        double[] operands = new double[operandCount];
                        while (operandCount-- > 0)
                        {
                            if (operandStack.Count == 0)
                            {
                                validOperation = false;
                                lessOperand = true;
                            }
                            if(validOperation) operands[operandCount] = (double)operandStack.Pop().Value;
                        }
                        if (!validOperation) break;
                        double result = operatorClass.Calculate(operands);
                        operandStack.Push(new Token { Value = result, TokenType = TokenType.Operand });
                    }
                }
            }
            if (lessOperand)
            {
                throw new NumberOfOperandInOperationException(error.GetString("TooLessOperand")) { Actual = actualOperands, Expected = expectedOperands };
            }
            if (operandStack.Count > 1)
            {
                throw new NumberOfOperandInOperationException(error.GetString("TooManyOperand")) { Actual = actualOperands, Expected = expectedOperands };
            }
            return (double)operandStack.Pop().Value;
        }
        public double Evaluate(string expression, Memory operation)
        {
            double result = Evaluate(expression);
            switch (operation)
            {
                case Memory.Add:
                    MemoryOperation.Add(result);
                    break;
                case Memory.Subtract:
                    MemoryOperation.Subtract(result);
                    break;
                case Memory.Store:
                    MemoryOperation.Store(result);
                    break;
                case Memory.Recall:
                    MemoryOperation.Recall();
                    break;
                case Memory.Clear:
                    MemoryOperation.Clear();
                    break;
            }
            return result;
        }
        public double Evaluate(string expression, Memory operation, int index)
        {
            double result = Evaluate(expression);
            switch (operation)
            {
                case Memory.Add:
                    MemoryOperation.Add(result, index);
                    break;
                case Memory.Subtract:
                    MemoryOperation.Subtract(result, index);
                    break;
                case Memory.Store:
                    throw new NotImplementedException();
                case Memory.Recall:
                    MemoryOperation.Recall(index);
                    break;
                case Memory.Clear:
                    MemoryOperation.Clear(index);
                    break;
            }
            return result;
        }
    }   
}