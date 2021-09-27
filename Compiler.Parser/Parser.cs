using Compiler.Core;
using Compiler.Core.Expressions;
using Compiler.Core.Interfaces;
using Compiler.Core.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using Type = Compiler.Core.Type;

namespace Compiler.Parser
{
    public class Parser : IParser
    {
        private readonly IScanner scanner;
        private Token lookAhead;

        public Parser(IScanner scanner)
        {
            this.scanner = scanner;
            this.Move();
        }

        public Statement Parse()
        {
            return Program2();
        }

        private Statement Program()
        {
            EnvironmentManager.PushContext();
            EnvironmentManager.AddMethod(
                "print", 
                new Id(
                        new Token
                        {
                            Lexeme = "print",
                        }, 
                        Type.Void),
            new ArgumentExpression(
                        new Token
                         {
                            Lexeme = ""
                        },
            new Id(new Token
            {
                Lexeme = "arg1"
            }, Type.String)));

            var block = Block();
            block.ValidateSemantic();
            var code = block.Generate(0);
            //code = code.Replace($"else:{Environment.NewLine}\tif", "elif");
            Console.WriteLine(code);
            return block;
        }

        private Statement Program2()
        {
            EnvironmentManager.PushContext();
            EnvironmentManager.AddMethod(
                "print",
                new Id(
                        new Token
                        {
                            Lexeme = "print",
                        },
                        Type.Void),
            new ArgumentExpression(
                        new Token
                        {
                            Lexeme = ""
                        },
            new Id(new Token
            {
                Lexeme = "arg1"
            }, Type.String)));

            var classs = Classs();
            classs.ValidateSemantic();
            var code = classs.Generate(0);
            //code = code.Replace($"else:{Environment.NewLine}\tif", "elif");
            Console.WriteLine(code);
            return classs;
        }

        private Statement Classs()
        {
            Match(TokenType.Class);
            EnvironmentManager.PushContext();
            var classname = this.lookAhead;
            Match(TokenType.Identifier);
            Match(TokenType.OpenBrace);
            var statements = Mtds();
            Match(TokenType.CloseBrace);
            EnvironmentManager.PopContext();
            return new ClasssStatement(classname,statements);
        }

        private Statement Mtds()
        {
            if (this.lookAhead.TokenType == TokenType.CloseBrace)
            {//{}
                return null;
            }
            return new SequenceStatement(Mtd(), Mtds());
        }

        private Statement Mtd()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.IntKeyword:
                    Match(TokenType.IntKeyword);
                    break;
                case TokenType.FloatKeyword:
                    Match(TokenType.FloatKeyword);
                    break;
                case TokenType.StringKeyword:
                    Match(TokenType.StringKeyword);
                    break;
                case TokenType.BoolKeyword:
                    Match(TokenType.BoolKeyword);
                    break;
                case TokenType.DateKeyword:
                    Match(TokenType.DateKeyword);
                    break;
                case TokenType.Void:
                    Match(TokenType.Void);
                    break;

            }
            var id = lookAhead;
            Match(TokenType.Identifier);
            Match(TokenType.LeftParens);
            var expression = OptParams();
            Match(TokenType.RightParens);
            var statement = Block();
            return new MethodStatement(id,expression as TypedExpression,statement);
        }
        private Statement Block()
        {
            Match(TokenType.OpenBrace);
            EnvironmentManager.PushContext();
            Decls();
            var statements = Stmts();
            Match(TokenType.CloseBrace);
            EnvironmentManager.PopContext();
            return statements;
        }

        private Statement Stmts()
        {
            if (this.lookAhead.TokenType == TokenType.CloseBrace)
            {//{}
                return null;
            }
            return new SequenceStatement(Stmt(), Stmts());
        }

        private Statement Stmt()
        {
            Expression expression;
            Statement statement1, statement2;
            switch (this.lookAhead.TokenType)
            {
                case TokenType.Identifier:
                    {
                        var symbol = EnvironmentManager.GetSymbol(this.lookAhead.Lexeme);
                        Match(TokenType.Identifier);
                        if (this.lookAhead.TokenType == TokenType.Assignation)
                        {
                            return AssignStmt(symbol.Id);
                        }else if(this.lookAhead.TokenType == TokenType.Incremental)
                        {

                        }else if(this.lookAhead.TokenType == TokenType.Decremental)
                        {
                            symbol.ToString();
                            
                        }
                        return CallStmt(symbol);
                    }
                case TokenType.IfKeyword:
                    {
                        Match(TokenType.IfKeyword);
                        Match(TokenType.LeftParens);
                        //expression = Eq();
                        expression = Logical();
                        Match(TokenType.RightParens);
                        statement1 = Stmt();
                        if (this.lookAhead.TokenType != TokenType.ElseKeyword)
                        {
                            return new IfStatement(expression as TypedExpression, statement1);
                        }
                        Match(TokenType.ElseKeyword);
                        statement2 = Stmt();
                        return new ElseStatement(expression as TypedExpression, statement1, statement2);
                    }
                case TokenType.WhileKeyword:
                    {
                        Match(TokenType.WhileKeyword);
                        Match(TokenType.LeftParens);
                        //expression = Eq();
                        expression = Logical();
                        Match(TokenType.RightParens);
                        statement1 = Block(); //es bloque sarita no te estreses wey

                        return new WhileStatement(expression as TypedExpression, statement1);
                    }
                case TokenType.ReturnKeyword:
                    {
                        Match(TokenType.ReturnKeyword);
                        //expression = Eq();
                        expression = Logical();
                        Match(TokenType.SemiColon);

                        return new ReturnStatement(expression as TypedExpression);
                    }
                case TokenType.ForeachKeyword:
                    {
                        Match(TokenType.ForeachKeyword);
                        Match(TokenType.LeftParens);
                        var tipo = this.lookAhead;
                        Token variablename=null;
                        Id id = null;
                        switch (this.lookAhead.TokenType)
                        {
                            case TokenType.IntKeyword:
                                Match(TokenType.IntKeyword);
                                id = new Id(tipo, Type.Int);
                                variablename = this.lookAhead;
                                break;
                            case TokenType.FloatKeyword:
                                Match(TokenType.FloatKeyword);
                                id = new Id(tipo, Type.Float);
                                variablename = this.lookAhead;
                                break;
                            case TokenType.StringKeyword:
                                Match(TokenType.StringKeyword);
                                id = new Id(tipo, Type.String);
                                variablename = this.lookAhead;
                                break;
                            case TokenType.BoolKeyword:
                                Match(TokenType.BoolKeyword);
                                id = new Id(tipo, Type.Bool);
                                variablename = this.lookAhead;
                                break;
                            case TokenType.DateKeyword:
                                Match(TokenType.DateKeyword);
                                id = new Id(tipo, Type.Date);
                                variablename = this.lookAhead;
                                break;
                            default:
                                throw new ApplicationException("Type required in foreach");
                        }
                        EnvironmentManager.AddVariable(variablename.Lexeme, id);

                        Match(TokenType.Identifier);
                        Match(TokenType.InKeyword);
                        var listname = this.lookAhead;
                        Match(TokenType.Identifier);
                        Match(TokenType.RightParens);
                        statement1 = Block();
                        return new ForeachStatement(tipo,variablename,listname,statement1);
                    }
                default:
                    return Block();
            }
        }
        private Expression Logical()
        {
            var expression = Eq();
            while (this.lookAhead.TokenType == TokenType.Or || this.lookAhead.TokenType == TokenType.And)
            {
                var token = lookAhead;
                Move();
                expression = new ConditionalExpression(token, expression as TypedExpression, Eq() as TypedExpression);
                
            }

            return expression;
        }

        private Expression Eq()
        {
            var expression = Rel();
            while (this.lookAhead.TokenType == TokenType.Equal || this.lookAhead.TokenType == TokenType.NotEqual)
            {
                var token = lookAhead;
                Move();
                expression = new RelationalExpression(token, expression as TypedExpression, Rel() as TypedExpression);
            }

            return expression;
        }

        private Expression Rel()
        {
            var expression = Expr();
            if (this.lookAhead.TokenType == TokenType.LessThan
                || this.lookAhead.TokenType == TokenType.GreaterThan
                || this.lookAhead.TokenType == TokenType.LessOrEqualThan
                || this.lookAhead.TokenType == TokenType.GreaterOrEqualThan)
            {
                var token = lookAhead;
                Move();
                expression = new RelationalExpression(token, expression as TypedExpression, Expr() as TypedExpression);
            }
            return expression;
        }

        private Expression Expr()
        {
            var expression = Term();
            while (this.lookAhead.TokenType == TokenType.Plus || this.lookAhead.TokenType == TokenType.Minus)
            {
                var token = lookAhead;
                Move();
                expression = new ArithmeticOperator(token, expression as TypedExpression, Term() as TypedExpression);
            }
            return expression;
        }

        private Expression Term()
        {
            var expression = Factor();
            while (this.lookAhead.TokenType == TokenType.Asterisk || this.lookAhead.TokenType == TokenType.Division || this.lookAhead.TokenType == TokenType.Mod)
            {
                var token = lookAhead;
                Move();
                expression = new ArithmeticOperator(token, expression as TypedExpression, Factor() as TypedExpression);
            }
            return expression;
        }

        private Expression Factor()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.LeftParens:
                    {
                        Match(TokenType.LeftParens);
                        //var expression = Eq();
                        var expression = Logical();
                        Match(TokenType.RightParens);
                        return expression;
                    }
                case TokenType.IntConstant:
                    var constant = new Constant(lookAhead, Type.Int);
                    Match(TokenType.IntConstant);
                    return constant;
                case TokenType.FloatConstant:
                    constant = new Constant(lookAhead, Type.Float);
                    Match(TokenType.FloatConstant);
                    return constant;
                case TokenType.StringConstant:
                    constant = new Constant(lookAhead, Type.String);
                    Match(TokenType.StringConstant);
                    return constant;
                case TokenType.BoolConstant:
                    constant = new Constant(lookAhead, Type.Bool);
                    Match(TokenType.BoolConstant);
                    return constant;
                case TokenType.Not:
                    Match(TokenType.Not);
                    return Logical();
                case TokenType.DateConstant:
                    var token2 = this.lookAhead;
                    List<Token> list = new List<Token>();
                    //list.Add(lookAhead);
                    Match(TokenType.DateConstant);
                    list.Add(lookAhead);
                    Match(TokenType.LeftParens);
                    list.Add(lookAhead);
                    Match(TokenType.IntConstant);
                    //list.Add(lookAhead);
                    Match(TokenType.Division);
                    list.Add(lookAhead);
                    Match(TokenType.IntConstant);
                    //list.Add(lookAhead);
                    Match(TokenType.Division); 
                    list.Add(lookAhead);
                    Match(TokenType.IntConstant);
                    list.Add(lookAhead);
                    Match(TokenType.RightParens);
                    return new ConstantExpression(token2, Type.Date, string.Concat(list.Select(x => x.Lexeme)));
                case TokenType.ListKeyword:

                    token2 = this.lookAhead;
                    list = new List<Token>();
                    //list.Add(lookAhead);
                    Match(TokenType.ListKeyword);
                    Match(TokenType.LessThan);
                    var tipo = this.lookAhead;
                    switch (tipo.TokenType)
                    {
                        case TokenType.IntKeyword:
                            {
                                Match(TokenType.IntKeyword);
                                break;
                            }
                        case TokenType.FloatKeyword:
                            {
                                Match(TokenType.FloatKeyword);
                                break;
                            }
                        case TokenType.BoolKeyword:
                            {
                                Match(TokenType.BoolKeyword);
                                break;
                            }
                        case TokenType.StringKeyword:
                            {
                                Match(TokenType.StringKeyword);
                                break;
                            }
                    }
                    Match(TokenType.GreaterThan);
                    //list.Add(lookAhead);
                    Match(TokenType.LeftParens);
                    list.Add(lookAhead);
                    switch (tipo.TokenType)
                    {
                        case TokenType.IntKeyword:
                            {
                                Match(TokenType.IntConstant);
                                while (lookAhead.TokenType != TokenType.RightParens)
                                {
                                    list.Add(lookAhead);
                                    Match(TokenType.Comma);
                                    list.Add(lookAhead);
                                    Match(TokenType.IntConstant);
                                }
                                Match(TokenType.RightParens);
                                return new ConstantExpression(token2, Type.ListInt, string.Concat(list.Select(x => x.Lexeme)));
                            }
                        case TokenType.FloatKeyword:
                            {
                                Match(TokenType.FloatConstant);
                                while (lookAhead.TokenType != TokenType.RightParens)
                                {
                                    list.Add(lookAhead);
                                    Match(TokenType.Comma);
                                    list.Add(lookAhead);
                                    Match(TokenType.FloatConstant);
                                }
                                Match(TokenType.RightParens);
                                return new ConstantExpression(token2, Type.ListFloat, string.Concat(list.Select(x => x.Lexeme)));
                            }
                        case TokenType.BoolKeyword:
                            {
                                Match(TokenType.BoolConstant);
                                while (lookAhead.TokenType != TokenType.RightParens)
                                {
                                    list.Add(lookAhead);
                                    Match(TokenType.Comma);
                                    list.Add(lookAhead);
                                    Match(TokenType.BoolConstant);
                                }
                                Match(TokenType.RightParens);
                                return new ConstantExpression(token2, Type.ListBool, string.Concat(list.Select(x => x.Lexeme)));
                            }
                        case TokenType.StringKeyword:
                            {
                                Match(TokenType.StringConstant);
                                while (lookAhead.TokenType != TokenType.RightParens)
                                {
                                    list.Add(lookAhead);
                                    Match(TokenType.Comma);
                                    list.Add(lookAhead);
                                    Match(TokenType.StringConstant);
                                }
                                Match(TokenType.RightParens);
                                return new ConstantExpression(token2, Type.ListString, string.Concat(list.Select(x => x.Lexeme)));
                            }
                    }
                    //list.Add(lookAhead);
                    Match(TokenType.RightParens);
                    return new ConstantExpression(token2, Type.List, string.Concat(list.Select(x => x.Lexeme)));
                default:
                    var symbol = EnvironmentManager.GetSymbol(this.lookAhead.Lexeme);
                    Match(TokenType.Identifier);
                    return symbol.Id;
            }
        }

        private Statement CallStmt(Symbol symbol)
        {
            Match(TokenType.LeftParens);
            var @params = OptParams();
            Match(TokenType.RightParens);
            Match(TokenType.SemiColon);
            return new CallStatement(symbol.Id, @params, symbol.Attributes);
        }

        private Expression OptParams()
        {
            if (this.lookAhead.TokenType != TokenType.RightParens)
            {
                return Params();
            }
            return null;
        }

        private Expression Params()
        {
            //var expression = Eq();
            var expression = Logical();
            if (this.lookAhead.TokenType != TokenType.Comma)
            {
                return expression;
            }
            Match(TokenType.Comma);
            expression = new ArgumentExpression(lookAhead, expression as TypedExpression, Params() as TypedExpression);
            return expression;
        }

        private Statement AssignStmt(Id id)
        {            
            Match(TokenType.Assignation);
            //var expression = Eq();
            var expression = Logical();
            Match(TokenType.SemiColon);
            return new AssignationStatement(id, expression as TypedExpression);
        }

        private Statement IncrementalStmt(Id id)
        {
            Match(TokenType.Incremental);
            //var expression = Eq();
            //var expression = Logical();
            Match(TokenType.SemiColon);
            //return new AssignationStatement(id, expression as TypedExpression);
            return null;
        }

        private void Decls()
        {
            if (this.lookAhead.TokenType == TokenType.IntKeyword || 
                this.lookAhead.TokenType == TokenType.ListKeyword ||
                this.lookAhead.TokenType == TokenType.FloatKeyword ||
                this.lookAhead.TokenType == TokenType.StringKeyword ||
                this.lookAhead.TokenType == TokenType.BoolKeyword ||
                this.lookAhead.TokenType == TokenType.DateKeyword
                )
            {
                Decl();
                Decls();
            }
        }

        private void Decl()
        {
            switch (this.lookAhead.TokenType)
            {
                case TokenType.FloatKeyword:
                    Match(TokenType.FloatKeyword);
                    var token = lookAhead;
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    var id = new Id(token, Type.Float);
                    EnvironmentManager.AddVariable(token.Lexeme, id);
                    break;
                case TokenType.StringKeyword:
                    Match(TokenType.StringKeyword);
                    token = lookAhead;
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    id = new Id(token, Type.String);
                    EnvironmentManager.AddVariable(token.Lexeme, id);
                    break;
                case TokenType.DateKeyword:
                    Match(TokenType.DateKeyword);
                    token = lookAhead;
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    id = new Id(token, Type.Date);
                    EnvironmentManager.AddVariable(token.Lexeme, id);
                    break;
                case TokenType.BoolKeyword:
                    Match(TokenType.BoolKeyword);
                    token = lookAhead;
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    id = new Id(token, Type.Bool);
                    EnvironmentManager.AddVariable(token.Lexeme, id);
                    break;
                case TokenType.ListKeyword:
                    Match(TokenType.ListKeyword);
                    Match(TokenType.LessThan);
                    var type = lookAhead;
                    switch (type.TokenType)
                        {
                        case TokenType.IntKeyword:
                            {
                                Match(TokenType.IntKeyword);
                                Match(TokenType.GreaterThan);
                                token = lookAhead;
                                Match(TokenType.Identifier);
                                Match(TokenType.SemiColon);
                                id = new Id(token, Type.ListInt);
                                EnvironmentManager.AddVariable(token.Lexeme, id);
                                break;
                            }
                        case TokenType.FloatKeyword:
                            {
                                Match(TokenType.FloatKeyword);
                                Match(TokenType.GreaterThan);
                                token = lookAhead;
                                Match(TokenType.Identifier);
                                Match(TokenType.SemiColon);
                                id = new Id(token, Type.ListFloat);
                                EnvironmentManager.AddVariable(token.Lexeme, id);
                                break;
                            }
                        case TokenType.BoolKeyword:
                            {
                                Match(TokenType.BoolKeyword);
                                Match(TokenType.GreaterThan);
                                token = lookAhead;
                                Match(TokenType.Identifier);
                                Match(TokenType.SemiColon);
                                id = new Id(token, Type.ListBool);
                                EnvironmentManager.AddVariable(token.Lexeme, id);
                                break;
                            }
                        case TokenType.StringKeyword:
                            {
                                Match(TokenType.StringKeyword);
                                Match(TokenType.GreaterThan);
                                token = lookAhead;
                                Match(TokenType.Identifier);
                                Match(TokenType.SemiColon);
                                id = new Id(token, Type.ListString);
                                EnvironmentManager.AddVariable(token.Lexeme, id);
                                break;
                            }
                    }
                    
                    break;
                default:
                    Match(TokenType.IntKeyword);
                    token = lookAhead;
                    Match(TokenType.Identifier);
                    Match(TokenType.SemiColon);
                    id = new Id(token, Type.Int);
                    EnvironmentManager.AddVariable(token.Lexeme, id);
                    break;
            }
        }

        private void Move()
        {
            this.lookAhead = this.scanner.GetNextToken();
        }

        private void Match(TokenType tokenType)
        {
            if (this.lookAhead.TokenType != tokenType)
            {
                throw new ApplicationException($"Syntax error! expected token {tokenType} but found {this.lookAhead.TokenType}. Line: {this.lookAhead.Line}, Column: {this.lookAhead.Column}");
            }
            this.Move();
        }
    }
}
