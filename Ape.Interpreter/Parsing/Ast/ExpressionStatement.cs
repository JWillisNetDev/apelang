using Ape.Interpreter.Lexing;

namespace Ape.Interpreter.Parsing.Ast;

public class ExpressionStatement : Statement
{
    public override StatementType Type => StatementType.ExpressionStatement;
    public Expression Expression { get; }

    public ExpressionStatement(Token token, Expression expression) : base(token)
    {
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));
    }
}