using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdktlCompiler
{
    public record TokenTuple(Boolean Success, String? Token, TokenType? TokenType);

    public record TokenReducerResponse(Boolean Error, Boolean Continue, String Token);

    internal class Lexer
    {
        private int _cursorPosition = 0;
        private readonly String _text;

        public Lexer(String inputProgram)
        {
            _text = inputProgram.Replace("\n", "");
        }
        
        public static char[] CharNumberList => new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        public static char[] CharLetterList => new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'};
        public static char[] CharMathOperatorList => new[] { '+', '-', '*', '/' };
        public static String[] KeywordList => new[] { "for", "if", "else", "then", "when", "switch" };

        public TokenTuple GetNextToken()
        {
            var tokenType = IdentifyTokenType();

            return tokenType switch
            {
                TokenType._ALPHANUMERIC => Reducer(ReadAlphanumeric, TokenType._ALPHANUMERIC),
                TokenType.NUMBER => Reducer(ReadNumber, TokenType.NUMBER),
                TokenType.STRING => Reducer(ReadString, TokenType.STRING),
                TokenType x => Reducer(ReadSingleCharToken, x),
                _ => new TokenTuple(false, null, null)
            };
        }

        public IEnumerable<TokenTuple> GetTokenIterator()
        {
            while (true)
            {
                TokenTuple token = GetNextToken();

                if (!token.Success || token.Token == "")
                {
                    yield break;
                }
                
                yield return token;
            }
        }

        private static TokenType? GetTokenTypeFromInitialChar(char c)
        {
            return c switch
            {
                var x when IsALetter(x) => TokenType._ALPHANUMERIC,
                var x when IsANumber(x) => TokenType.NUMBER,
                var x when IsAMathOperator(x) => TokenType.MATH_OPERATOR,
                '"' => TokenType.STRING,
                ',' => TokenType.COMMA,
                ';' => TokenType.SEMICOLON,
                ':' => TokenType.COLON,
                '(' => TokenType.OPEN_PARENTHESIS,
                ')' => TokenType.CLOSE_PARENTHESIS,
                '{' => TokenType.OPEN_BRACKET,
                '}' => TokenType.CLOSE_BRACKET,
                '=' => TokenType.ASSIGNMENT,
                _ => null
            };
        }

        private TokenType? IdentifyTokenType()
        {
            for (; _cursorPosition < _text.Length; _cursorPosition++)
            {
                char currentChar = _text[_cursorPosition];

                if (currentChar != ' ')
                    return GetTokenTypeFromInitialChar(currentChar);
            }

            return null;
        }

        private TokenReducerResponse ReadNumber(String token, Char currentChar)
        {
            if (!IsANumber(currentChar))
            {
                return new TokenReducerResponse(false, false, token);
            }

            return new TokenReducerResponse(false, true, token + currentChar);
        }

        private TokenReducerResponse ReadAlphanumeric(String token, Char currentChar)
        {
            if (!IsALetter(currentChar) && !IsANumber(currentChar))
            {
                return new TokenReducerResponse(false, false, token);
            }
            
            return new TokenReducerResponse(false, true, token + currentChar);
        }

        private TokenReducerResponse ReadString(String token, Char currentChar)
        {
            if (!token.Equals("") && currentChar == '"')
            {
                _cursorPosition++; // Consumes " char
                return new TokenReducerResponse(false, false, token + '"');
            }

            if (currentChar == '\\')
            {
                _cursorPosition++;
                return new TokenReducerResponse(false, true,  token + _text[_cursorPosition]);
            }

            return new TokenReducerResponse(false, true, token + currentChar);
        }

        private TokenReducerResponse ReadSingleCharToken(String token, Char currentChar)
        {
            _cursorPosition++; // Consumes char
            return new TokenReducerResponse(false, false, token + currentChar);
        }
        
        private TokenTuple Reducer(Func<String, Char, TokenReducerResponse> function, TokenType tokenType)
        {
            var token = String.Empty;

            for (; _cursorPosition < _text.Length; _cursorPosition++)
            {
                char currentChar = _text[_cursorPosition];

                TokenReducerResponse response = function(token, currentChar);
                
                if (!response.Error)
                {
                    token = response.Token;
                } 
                else
                {
                    return new TokenTuple(true, token, tokenType);
                }

                if (!response.Continue)
                {
                    break;
                }
            }

            // An _ALPHANUMERIC token got to be classified as KEYWORD or IDENTIFIER
            if (tokenType == TokenType._ALPHANUMERIC)
            {
                if (KeywordList.Contains(token))
                {
                    tokenType = TokenType.KEYWORD;
                }
                else
                {
                    tokenType = TokenType.IDENTIFIER;
                }
            }

            return new TokenTuple(true, token, tokenType);
        }

        private static bool IsANumber(char c)
        {
            return CharNumberList.Contains(c);
        }

        private static bool IsALetter(char c)
        {
            return CharLetterList.Contains(c);
        }

        private static bool IsAMathOperator(char c)
        {
            return CharMathOperatorList.Contains(c);
        }
    }
}
