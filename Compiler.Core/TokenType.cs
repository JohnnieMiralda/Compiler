using System;
using System.Collections.Generic;
using System.Text;

namespace Compiler.Core
{
    public enum TokenType
    {
    //nose como llamarlos
        LeftParens,
        RightParens,
        SemiColon,
        EOF,
        OpenBrace,
        CloseBrace,
        Comma,
        Identifier,
        Class,
        Void,
        New,
     //tipos de datos
        ListKeyword,
        BasicType,
        IntKeyword,
        IntConstant,
        IntListConstant,
        FloatKeyword,
        FloatConstant,
        FloatListKeyword,
        FloatListConstant,
        BoolKeyword,
        BoolConstant,
        BoolListKeyword,
        BoolListConstant,
        StringKeyword,
        StringConstant,
        StringListKeyword,
        StringListConstant,
        DateKeyword,
        DateConstant,
        YearKeyword,
        MonthKeyword,
        DayKeyword,
        TrueKeyword,
        FalseKeyword,
     //tipos de operacciones
        //aritmeticas
        Plus,
        Minus,
        Asterisk,
        Division,
        Mod,
        //relacionales
        Equal,
        NotEqual,
        GreaterThan,
        LessThan,
        GreaterOrEqualThan,
        LessOrEqualThan,
        //logicas
        And,
        Or,
        Not,
        //asignacion, incremento y decremento
        Assignation,
        Incremental,
        Decremental,
     //Sentencias
        ForeachKeyword,
        InKeyword,
        WhileKeyword,
        IfKeyword,
        ElseKeyword,
    //Methods
        


    }
}
