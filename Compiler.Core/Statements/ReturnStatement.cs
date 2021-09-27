using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;
using Compiler.Core.Interfaces;

namespace Compiler.Core.Statements
{
    public class ReturnStatement : Statement
    {
        public ReturnStatement(TypedExpression expression)
        {
            Expression = expression;
        }

        public TypedExpression Expression { get; }

        public override string Generate(int tab)
        {
            var code = GetCodeInit(tab);
            code += $"return {Expression.Generate()};{Environment.NewLine}";
            return code;
        }

        public override void Interpret()
        {
            throw new NotImplementedException();
        }

        public override void ValidateSemantic()
        {
            if (Expression == null)
            {
                throw new ApplicationException($"Return value can not be null");
            }
        }
    }
}
