using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;
using System.Resources;
using System.Reflection;

namespace MathLibrary
{
    public class ParseExpression
    {
        private static readonly ResourceManager error;
        static ParseExpression()
        {
            error = new ResourceManager("MathLibrary.ExceptionMessage", Assembly.GetExecutingAssembly());
        }
        public static List<Token> GetTokens(string expression, List<Operator> operators)
        {
            List<Token> tokens = new List<Token>();
            ParseExpression parse = new ParseExpression();
            for(int expressionIndex = 0; expressionIndex < expression.Length; expressionIndex++)
            {
                string token = "";
                if (Char.IsDigit(expression[expressionIndex]) || expression[expressionIndex] == '.')
                {
                    while (expressionIndex < expression.Length && (Char.IsDigit(expression[expressionIndex]) || expression[expressionIndex] == '.'))
                    {
                        token += expression[expressionIndex];
                        expressionIndex++;
                    }
                    expressionIndex--;
                    tokens.Add(new Token { Value = Double.Parse(token), TokenType = TokenType.Operand });
                }
                else if (expression[expressionIndex]=='(' )
                {
                    tokens.Add(new Token { Value = "(", TokenType = TokenType.Operator });
                }
                else if (expression[expressionIndex] == ')')
                {
                    tokens.Add(new Token { Value = ")", TokenType = TokenType.Operator });
                }
                else
                {
                    while (expressionIndex < expression.Length && !Char.IsDigit(expression[expressionIndex]) && expression[expressionIndex] != '.' 
                        && expression[expressionIndex] != '(' && expression[expressionIndex] != ')')
                    {
                        token += expression[expressionIndex];
                        expressionIndex++;
                        if (operators.Where(x => x.Symbol==token).Count()>0) break;
                    }
                    expressionIndex--;
                    tokens.Add(new Token { Value = token, TokenType = TokenType.Operator });
                }
            }
            return tokens;
        }
        public static List<Operator> GetOperatorTable()
        {
            //To change text file to a Dictionary
            
            List<Operator> operators = new List<Operator>();
            using(StreamReader precedenceTableOfOperator = new StreamReader("PrecedenceTableOfOperator.json"))
            { 
                operators = JsonSerializer.Deserialize<List<Operator>>(precedenceTableOfOperator.ReadToEnd());
            }
            return operators;
        }
        public static List<Token> ToPostfix(string expression)
        {
            List<Operator> operators = GetOperatorTable();
            return ToPostfix(expression, operators);
        }
        public static List<Token> ToPostfix(string expression, List<Operator> operators)
        {
            List<Token> tokens = GetTokens(expression, operators);

            //postfix logic

            List<Token> postfixExpression = new List<Token>();
            Stack<Token> stack = new Stack<Token>();

            foreach (Token token in tokens)
            {
                if(token.TokenType == TokenType.Operand)    //operand is encountered
                {
                    postfixExpression.Add(token);
                }
                else if((string)token.Value == "(")
                {
                    stack.Push(token);
                }
                else if((string)token.Value == ")")
                {
                    while (stack.Count > 0 && (string)stack.Peek().Value != "(")
                    {
                        postfixExpression.Add(stack.Pop());
                    }

                    if (stack.Count > 0 && (string)stack.Peek().Value != "(")
                    {
                        throw new InvalidExpressionException(error.GetString("InvalidExpression"));
                    }
                    else
                    {
                        stack.Pop();
                    }
                }
                else    //operator is encountered
                {
                    while (stack.Count > 0 && (string)stack.Peek().Value != "(" && (string)stack.Peek().Value != ")"
                        && ComparePrecedence(token, stack.Peek(), operators))
                    {
                        postfixExpression.Add(stack.Pop());
                    }
                    stack.Push(token);
                }
            }
            // pop all the operators from the stack
            while (stack.Count > 0)
            {
                postfixExpression.Add(stack.Pop());
            }

            return postfixExpression;
        }
        private static bool ComparePrecedence(Token firstToken, Token secondToken, List<Operator> operators)
        {
            int firstPrecedence = operators.Where(a => a.Symbol == (string)firstToken.Value).First().Precedence;
            int secondPrecedence = operators.Where(a => a.Symbol == (string)secondToken.Value).First().Precedence;
            if (firstPrecedence == 1 && secondPrecedence == 1)
                return false;
            else if (firstPrecedence >= secondPrecedence)
                return true;
            return false;
        }
    }
}