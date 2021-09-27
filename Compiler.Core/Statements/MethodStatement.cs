using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class MethodStatement : Statement
    {
        public MethodStatement(Token name,TypedExpression expression, Statement block)
        {
            Name = name;
            Expression = expression;
            Block = block;
        }

        public Token Name { get; }
        public TypedExpression Expression { get; }
        public Statement Block { get; }

        public override string Generate(int tab)
        {
            var code = GetCodeInit(tab);
            code += $"function {Name.Lexeme}({Expression?.Generate()}){Environment.NewLine}";// {{{Environment.NewLine}";
            for (int x = 0; x < tab; x++)
            {
                code += "\t";
            }
            code += $"{{{Environment.NewLine}";
            code += $"{Block.Generate(tab + 1)}";// {Environment.NewLine}}}{Environment.NewLine} ";
            for (int x = 0; x < tab; x++)
            {
                code += "\t";
            }
            code += $"}}{Environment.NewLine}";

            return code;
        }

        public override void Interpret()
        {
            if (Expression.Evaluate())
            {
                Block.Interpret();
            }
        }

        public override void ValidateSemantic()
        {
           
        }
    }
}
