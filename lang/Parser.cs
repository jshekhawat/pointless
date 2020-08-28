using System;
using System.Collections.Generic;
using System.Linq;

namespace lang {

    
    public class Parser {
        
        private List<Token> tokens;
        private int current = 0;


        public Parser(List<Token> tokens) {                
            this.tokens = tokens;            
        }

        public Expr Parse() {
            return expression();            
        }

        private Expr expression() => equality();                    
        

        private Expr equality()
        {
            Expr expr = comparison();
            
            while (match(new TokenType[] {
                TokenType.BANG_EQUAL,
                TokenType.EQUAL_EQUAL
            })) {
                Token op = previous();
                Expr right = comparison();
                expr = new Expr.Binary() {
                    Left = expr,
                    Right = right,
                    Operator = op
                };
            }   
            return expr;          
        }

        private Expr comparison()
        {
            Expr expr = addition();
            while(match(new TokenType[] {
                TokenType.GREATER,
                TokenType.GREATER_EQUAL,
                TokenType.LESS,
                TokenType.LESS_EQUAL
            })) {
                Token op = previous();
                Expr right = addition();

                expr = new Expr.Binary(){
                    Left=expr,
                    Operator=op,
                    Right=right
                };
            }
            return expr;
        }    

        private Expr addition()
        {
            Expr expr = multiplication();
            
            while(match(new TokenType[] {
                TokenType.PLUS,
                TokenType.MINUS
            })) {
                Token op = previous();
                Expr right = multiplication();
                expr = new Expr.Binary(){
                    Left=expr,
                    Operator=op,
                    Right=right
                };
            }
            
            return expr;
        }

        private Expr multiplication()
        {
            Expr expr = unary();

            while(match(new TokenType[] {
                TokenType.STAR,
                TokenType.SLASH
            })) {
                Token op = previous();
                Expr right = unary();
                expr = new Expr.Binary(){
                    Left=expr,
                    Operator=op,
                    Right=right
                };
            }
            
            return expr;


        }

        private Expr unary()
        {            
            
            if(match(new TokenType[] {
                TokenType.BANG,
                TokenType.MINUS
            })) {             
                Token op = previous();   
                Expr right = unary();
                return new Expr.Unary() {
                    Operator = op,
                    Right = right
                };
            }
            return primary();
        }

        private Expr primary()
        {
            if(match(new TokenType[] {TokenType.NUMBER,
                TokenType.STRING,
                TokenType.FALSE,
                TokenType.TRUE,
                TokenType.NIL
            })) 
            return new Expr.Literal() { TokenValue = previous().Literal };
            
            if(match(new TokenType[] {
                TokenType.LEFT_PAREN
            })) {
                Expr expr = expression();
                return new Expr.Grouping() {
                    Expression = expr
                };                
            }

        return null;
        }

        private bool match(TokenType[] types) {
            foreach(var t in types) {
                if(check(t)) {
                    advance();
                    return true;
                }
            }
            return false;
        }

        private bool isAtEnd() {
            return peek().Type == TokenType.EOF;
        }

        private Token peek()
        {
            return tokens[current];
        }

        private Token previous() {
            return tokens[current -1];
        }

        private void advance()
        {
            if(!isAtEnd()) {
                current++;
            }
        }

        private bool check(TokenType t)
        {
            return peek().Type == t;
        }

        
    }
}


