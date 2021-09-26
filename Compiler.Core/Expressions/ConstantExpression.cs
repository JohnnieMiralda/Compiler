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
            switch (type.Lexeme)
            {
                case "date":
                    return DateTime.Parse(Lexeme);
                case "list<int>":
                    return new Lexeme;
                case "list<float>":
                    return new Lexeme;
                case "list<bool>":
                    return new Lexeme;
                case "list<string>":
                    return new Lexeme;
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
