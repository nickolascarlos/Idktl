using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdktlCompiler
{
    public enum TokenType
    {
        _ALPHANUMERIC,
        KEYWORD,
        IDENTIFIER,
        NUMBER,
        COMMA,
        SEMICOLON,
        COLON,
        OPEN_PARENTHESIS,
        CLOSE_PARENTHESIS,
        OPEN_BRACKET,
        CLOSE_BRACKET,
        STRING,
        ASSIGNMENT,
        MATH_OPERATOR,
    }
}
