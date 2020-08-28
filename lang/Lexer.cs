using System;
using System.Collections.Generic;

namespace lang {


    public class Token {

        String lexeme;
        TokenType type;

        Object literal;

        int line;


        public Token(TokenType type, String lexeme, Object literal, int line) {
            this.lexeme = lexeme;
            this.type = type;
            this.literal = literal;
            this.line = line;
        }

        public String Lexeme {
            get {
                return this.lexeme;
            }            
        }

        public TokenType Type {
            get {
                return this.type;
            }
        }

        public Object Literal {
            get {
                return this.literal;
            }
        }

        public int Line {
            get {
                return this.line;
            }
        }



    }
    
    public class Lexer {

        private Dictionary<String, TokenType> keywords = new Dictionary<string, TokenType>() {
            {"and", TokenType.AND},
            {"or", TokenType.OR},
            {"return", TokenType.RETURN},
            {"while", TokenType.WHILE},
            {"if", TokenType.IF},
            {"else", TokenType.ELSE},            
            {"for", TokenType.FOR},
            {"print", TokenType.PRINT},
            {"nil", TokenType.NIL},
            {"this", TokenType.THIS},
            {"true", TokenType.TRUE},
            {"super", TokenType.SUPER},
            {"false", TokenType.FALSE},
            {"break", TokenType.BREAK},
            {"var", TokenType.VAR},
            {"func", TokenType.FUNC},

        };

        
        String source;
        int current, start = 0;
        int line = 1;

        List<Token> tokens = new List<Token>();
        public Lexer(String source) {
            this.source = source;
        }

        public List<Token> Tokenise() {
            while(!isAtEnd()) {
                start = current;
                getToken();                
            }
            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        public void getToken()
        {

            char c = advance();            
            switch(c) {
                case '+': addToken(TokenType.PLUS); break;
                case '-': addToken(TokenType.MINUS); break;
                case '*': addToken(TokenType.STAR); break; 
                case '/': slash(); break;
                case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL :TokenType.EQUAL); break;
                case '!': addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
                case '>': addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;
                case '<': addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
                case '.': addToken(TokenType.DOT); break;
                case ';': addToken(TokenType.SEMICOLON); break;
                case '{': addToken(TokenType.LEFT_BRACE); break;
                case '}': addToken(TokenType.RIGHT_BRACE); break;
                case '(': addToken(TokenType.LEFT_PAREN); break;
                case ')': addToken(TokenType.RIGHT_PAREN); break;

                case '"': getString(); break;
                case '\n': line++; break;
                case '\r':
                case ' ' :
                case '\t':
                    break;            

                default:
                    if(isDigit(c)) {
                        number();
                    } else if(isAlpha(c)) {
                        identifier();
                    }
                    break;
            }
                        
        }

        private void getString() {
            while (!isAtEnd() && peek() != '"') {
                //allow multi line strings
                if(peek() == '\n') line++;
                advance();
            }
            if(isAtEnd()) {
                //TODO: Implement Errors
                return;
            }
            advance();
            String text = source.Substring(start + 1, ((current -1) - (start+1)));            
            addToken(TokenType.STRING, text);
        }

        private void identifier() {
            while(!isAtEnd() && isAlphaNumeric(peek())) advance();

            String text = source.Substring(start, (current - start));
            TokenType type;

            if(keywords.ContainsKey(text)) {
                type = keywords[text];
            } else {
                type = TokenType.IDENTIFIER;
            }

            addToken(type);

        }

        private bool isAlphaNumeric(char c) {
            return isAlpha(c) || isDigit(c);
        }

        
        private bool isAlpha(char c) {
            return (c >= 'a' && c <= 'z')
            || (c >= 'A' && c <= 'Z')
            || (c == '_');            
        }


        private void number() {
            // number format = [0-9]+.[0-9]+
            while(!isAtEnd() && isDigit(peek())) advance();


            if(peek() == '.' && isDigit(peekNext())) {
                advance();
                while(isDigit(peek())) advance();
            }

            addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, (current - start))));

        }

        private char peekNext() {
            if(current + 1 >= source.Length) return '\0';
            return source[current+1];
        }


        private bool isDigit(char c) {
            return c >= '0' && c <= '9';
        }
                
        private void slash() {
            if(match('/')) {
                while (peek() != '\n' && !isAtEnd()) advance();
                line++;
            } else {
                addToken(TokenType.SLASH);
            }
        }

        private char peek() {
            if(isAtEnd()) return '\0';
            return source[current];
        }

        private Boolean match(char c) {
            if(isAtEnd()) return false;
            if(source[current] != c) return false;            
            current++;
            return true;            
        }
    


        private void addToken(TokenType type) {            
            addToken(type, null);
            
        } 

        private void addToken(TokenType type, Object literal) {            
            String text = source.Substring(start, (current - start));             
            tokens.Add(new Token(type, text, literal, line));
        }

        private bool isAtEnd() {
            return current >= source.Length; 
        }

        //give back the chararacter and advance the source by 1

        private char advance() {            
                current++;
                return source[current - 1];                
        }


    }


    public enum TokenType {
        PRINT,
        PLUS,
        MINUS,
        STAR,
        MOD,
        SLASH,

        AND,
        OR,
        EQUAL_EQUAL,
        EQUAL,
        BANG,
        BANG_EQUAL,
        LESS,
        LESS_EQUAL,
        GREATER,
        GREATER_EQUAL,
        SUPER,
        TRUE,
        FALSE,
        STRING,
        IDENTIFIER,
        NUMBER,
        CLASS,
        VAR,
        SEMICOLON,
        FOR,
        IF,
        ELSE,        
        DOT,
        THIS,

        BREAK,
        LEFT_PAREN,
        LEFT_BRACE,
        RIGHT_PAREN,
        RIGHT_BRACE,
        COMMA,
        NIL,
        EOF,
        WHILE,
        FUNC,
        RETURN

    }

}