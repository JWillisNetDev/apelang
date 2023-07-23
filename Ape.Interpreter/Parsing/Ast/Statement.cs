using Ape.Interpreter.Lexing;

namespace Ape.Interpreter.Parsing.Ast;

public abstract class Statement : INode
{
    public abstract StatementType Type { get; }
    public string TokenLiteral { get; }
    
    protected Statement(Token token)
    {
        TokenLiteral = token.Literal.ToString();
    }
}