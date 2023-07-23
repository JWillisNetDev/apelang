using Ape.Interpreter.Lexing;

namespace Ape.Interpreter.Parsing.Ast;

public class IntegerLiteralExpression : LiteralExpression<int>
{
    public override ExpressionType Type => ExpressionType.IntegerLiteral;
    
    public IntegerLiteralExpression(Token token, int value) : base(token, value)
    {
    }
}