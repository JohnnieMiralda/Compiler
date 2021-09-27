using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ForeachStatement : Statement
    {
        public ForeachStatement(Token tipo,Token name ,Token listname, Statement block)
        {
            Tipo = tipo;
            Name = name;
            Listname = listname;
            Block = block;
        }

        public Token Tipo { get; }
        public Token Name { get; }
        public Token Listname { get; }
        public Statement Block { get; }

        public override string Generate(int tab)
        {
            var code = GetCodeInit(tab);
            code += $"for ( let {Name.Lexeme} in {Listname.Lexeme} ){Environment.NewLine}"; // {{{Environment.NewLine} ";
            for (int x = 0; x < tab; x++)
            {
                code += "\t";
            }
            code += $"{{{Environment.NewLine}";
            code += $"{Block.Generate(tab + 1)}{Environment.NewLine}";//     }}{Environment.NewLine} ";
            for (int x = 0; x < tab; x++)
            {
                code += "\t";
            }
            code += $"}}{Environment.NewLine}";
            return code;
        }

        public override void Interpret()
        {
            throw new NotImplementedException();
        }

        public override void ValidateSemantic()
        {
            if (Listname.TokenType != Tipo.TokenType)
            {
                throw new ApplicationException("Variable and list type must be the same type");
            }
        }
    }
}
