using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class DeclarationParamsStatement : Statement
    {
        public DeclarationParamsStatement(Id id)
        {
            Id = id;
        }

        public Id Id { get; }

        public override string Generate(int tab)
        {
            var code = GetCodeInit(tab);
            code += $"var {Id.Generate()},";
            return code;
        }

        public override void Interpret()
        {
            throw new NotImplementedException();
        }

        public override void ValidateSemantic()
        {
            if (Id.GetExpressionType() == Type.Bool || Id.GetExpressionType() == Type.String ||
                Id.GetExpressionType() == Type.Int || Id.GetExpressionType() == Type.Float ||
                Id.GetExpressionType() == Type.List
                )
            {
                throw new ApplicationException($"Type {Id.GetExpressionType()} is not assignable");
            }
        }
    }
}
