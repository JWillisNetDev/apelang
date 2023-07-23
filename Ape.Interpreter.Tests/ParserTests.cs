namespace Ape.Interpreter.Tests;

public class ParserTests
{


    [Fact]
    public void ParseProgram_ParsesValidProgram_CreatesAbstractSyntaxTree()
    {
        // Let statement - Identifier: 'foo' and Value Integer Literal = 123
        // Arrange
        const string input = "let foo = 123;";
        
        Lexer lexer = new(input);
        Parser parser = new(lexer);
        
        // Act
        Program actual = parser.ParseProgram();
        
        // Assert
        var letStatement = Assert.IsType<LetStatement>(Assert.Single(actual.Statements));
        
    }
    
    
}