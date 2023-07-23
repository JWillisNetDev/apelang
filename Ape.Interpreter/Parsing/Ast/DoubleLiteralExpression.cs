using Ape.Interpreter.Lexing;

namespace Ape.Interpreter.Parsing.Ast;

public class DoubleLiteralExpression : LiteralExpression<double>
{
    public override ExpressionType Type => ExpressionType.DoubleLiteral;

    public DoubleLiteralExpression(Token token, double value) : base(token, value)
    {
    }
}