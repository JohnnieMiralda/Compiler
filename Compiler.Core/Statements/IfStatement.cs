using Compiler.Core.Expressions;
using Compiler.Core.Interfaces;
using System;

namespace Compiler.Core.Statements
{
    public class IfStatement : Statement
    {
        public IfStatement(TypedExpression expression, Statement statement)
        {
            Expression = expression;
            Statement = statement;
        }

        public TypedExpression Expression { get; }
        public Statement Statement { get; }

        public override string Generate(int tabs)
        {
            var code = GetCodeInit(tabs);
            code += $"if({Expression.Generate()}):{Environment.NewLine}";// {{{Environment.NewLine}";
            for (int x = 0; x < tabs; x++)
            {
                code += "\t";
            }
            code += $"{{{Environment.NewLine}";
            code += $"{Statement.Generate(tabs + 1)}{Environment.NewLine}";//}}{Environment.NewLine}";
            for (int x = 0; x < tabs; x++)
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
                Statement.Interpret();
            }
        }

        public override void ValidateSemantic()
        {
            if (Expression.GetExpressionType() != Type.Bool)
            {
                throw new ApplicationException("A boolean is required in ifs");
            }
        }
    }
}
