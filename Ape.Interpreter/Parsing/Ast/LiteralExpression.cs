using Ape.Interpreter.Lexing;

namespace Ape.Interpreter.Parsing.Ast;

public abstract class LiteralExpression<T> : Expression
{
    public T Value { get; }
    protected LiteralExpression(Token token, T value) : base(token)
    {
        Value = value;
    }
}