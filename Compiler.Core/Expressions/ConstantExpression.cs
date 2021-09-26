using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class ConstantExpression : TypedExpression
    {
        public ConstantExpression(Token token, Type type, string lexeme) : base(token, type)
        {
            Lexeme = lexeme;
        }

        public string Lexeme { get; }

        public override dynamic Evaluate()
        {
            Console.WriteLine(type.Lexeme.ToString());
            switch (type.Lexeme.ToString())
            {
                case "datetime":
                    return DateTime.Parse(Lexeme);
                case "list<int>":
                    return  Lexeme;
                /*
                case "list<float>":
                    return new Lexeme;
                case "list<bool>":
                    return new Lexeme;
                case "list<string>":
                    return new Lexeme;*/
                case "year":
                case "month":
                case "day":
                    return int.Parse(Lexeme);
                default:
                    return null;
            }

        }

        public override string Generate()
        {
            if (Token.TokenType == TokenType.DateConstant)
            {   

                return $"new {Token.Lexeme}{Lexeme.ToString().Substring(0,3)}/{Lexeme.Substring(3, 2)}/{Lexeme.Substring(5, 5)}";
            }
            return Token.Lexeme;
        }
        // datetime carra = 07/10/1999;
        // var carra = new date(07/10/199);
        // array car=['datetime','carra','=','07/10/1999']
        //return $"var {array[1]} = new date({array[3})";

        public override Type GetExpressionType()
        {
            return type;
        }
    }
}
