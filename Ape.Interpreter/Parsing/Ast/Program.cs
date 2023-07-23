namespace Ape.Interpreter.Parsing.Ast;

public class Program : INode
{
    private readonly List<Statement> _statements = new();
    public IReadOnlyList<Statement> Statements => _statements;
    
    public string TokenLiteral => Statements.Any() ? Statements[0].TokenLiteral : string.Empty;

    internal Program()
    {
    }

    internal void AddStatement(Statement statement)
    {
        _statements.Add(statement);
    }
}