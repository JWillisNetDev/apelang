using Ape.Interpreter.Lexing;

namespace Ape.Interpreter.Parsing.Ast;

public class IdentifierExpression : Expression
{
    public string Name { get; }
    public override ExpressionType Type => ExpressionType.Identifier;

    public IdentifierExpression(Token token, string name) : base(token)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }
}