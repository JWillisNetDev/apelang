using Ape.Interpreter.Lexing;

namespace Ape.Interpreter.Parsing.Ast;

public class LetStatement : Statement
{
    public IdentifierExpression Identifier { get; }
    public Expression Value { get; }
    public override StatementType Type => StatementType.LetStatement;

    public LetStatement(Token token, IdentifierExpression identifier, Expression value) : base(token)
    {
        Identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }
}