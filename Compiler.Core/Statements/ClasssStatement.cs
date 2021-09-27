using Compiler.Core.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core.Statements
{
    public class ClasssStatement : Statement
    {
        public ClasssStatement(Token name, Statement methods)
        {
            Name = name;
            Methods = methods;
        }

        public Token Name { get; }
        public Statement Methods { get; }

        public override string Generate(int tab)
        {
            var code = GetCodeInit(tab);
            code += $"class {Name.Lexeme}{{{Environment.NewLine}{Methods?.Generate(tab + 1)}{Environment.NewLine}";    //}}{Environment.NewLine} ";
            for( int x =0; x < tab; x++)
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
            
        }
    }
}
