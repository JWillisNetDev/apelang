using System.Diagnostics.CodeAnalysis;

namespace Ape.Interpreter.Lexing;

public class Token
{
    public Token(TokenType type, string literal)
    {
        Type = type;
        Literal = literal;
    }

    public TokenType Type { get; init; }
    public string Literal { get; init; }
}