using Ape.Interpreter.Lexing;

namespace Ape.Interpreter.Parsing.Ast;

public abstract class Expression : INode
{
    public abstract ExpressionType Type { get; }
    public string TokenLiteral { get; }
    
    protected Expression(Token token)
    {
        TokenLiteral = token.Literal.ToString();
    }
}