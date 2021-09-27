using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class DecrementalStatement : Statement
    {
        public DecrementalStatement(Id id)
        {
            Id = id;
        }

        public Id Id { get; }

        public override string Generate(int tab)
        {
            var code = GetCodeInit(tab);
            code += $"{Id.Generate()}--;{Environment.NewLine}";
            return code;
        }

        public override void Interpret()
        {
            throw new NotImplementedException();
        }

        public override void ValidateSemantic()
        {
            if (Id.GetExpressionType() != Type.Int && Id.GetExpressionType() != Type.Float)
            {
                throw new ApplicationException($"Cannot increment type: {Id.GetExpressionType()}");
            }
        }
    }
}
