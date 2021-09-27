using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Core.Expressions
{
    public class ConstantExpression : TypedExpression
    {
        private int mos;
        private float mos2;
        private bool mos3;

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
                case "listint":
                    return Lexeme.Split(',').Where(m => int.TryParse(m, out mos)).Select(m => int.Parse(m)).ToList();
                case "listfloat":
                    return Lexeme.Split(',').Where(m => float.TryParse(m, out mos2)).Select(m => float.Parse(m)).ToList();
                case "listbool":
                    return Lexeme.Split(',').Where(m => bool.TryParse(m, out mos3)).Select(m => bool.Parse(m)).ToList();
                case "liststring":
                    return Lexeme.Split(',').ToList();


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
            switch (Token.TokenType)
            {
                case TokenType.DateConstant:
                    return $"new {Token.Lexeme}{Lexeme.ToString().Substring(0, 3)}/{Lexeme.Substring(3, 2)}/{Lexeme.Substring(5, 5)}";
                case TokenType.ListKeyword:
                    return $"[{Lexeme}]";
                default:
                    return Token.Lexeme;
            }
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
