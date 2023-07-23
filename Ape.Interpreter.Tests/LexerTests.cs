using static Ape.Interpreter.Lexing.TokenType;

namespace Ape.Interpreter.Tests;

public class LexerTests
{
    [Fact]
    public void LexerMoveNext_GivenValidInput_LexesTokens()
    {
        const string input = "let foo = 1.234;";
        
        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            (Let, "let"),
            (Identifier, "foo"),
            (Assign, "="),
            (TokenType.Double, "1.234"),
            (Semicolon, ";"),
            (EndOfFile, "\0"),
        });
    }

    [Fact]
    public void LexerMoveNext_GivenKeywordIdentifiers_LexesKeywordIdentifierTokens()
    {
        const string input = """
            fn let true false null if else for foreach return;
            """;

        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            (Function, "fn"),
            (Let, "let"),
            (True, "true"),
            (False, "false"),
            (Null, "null"),
            (If, "if"),
            (Else, "else"),
            (For, "for"),
            (ForEach, "foreach"),
            (Return, "return"),
            (Semicolon, ";"),
            (EndOfFile, "\0"),
        });
    }
    
    [Fact]
    public void LexerMoveNext_GivenDelimiters_LexesDelimiterTokens()
    {
        const string input = """
            (){}[],;:
            """;

        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            (OpenParen, "("),
            (CloseParen, ")"),
            (OpenBrace, "{"),
            (CloseBrace, "}"),
            (OpenBracket, "["),
            (CloseBracket, "]"),
            (Comma, ","),
            (Semicolon, ";"),
            (Colon, ":"),
            (EndOfFile, "\0"),
        });
    }
    
    [Fact]
    public void LexerMoveNext_GivenIdentifiers_LexesIdentifierTokens()
    {
        const string input = """
            foo bar test12345 some_variable_name
            """;

        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            (Identifier, "foo"),
            (Identifier, "bar"),
            (Identifier, "test12345"),
            (Identifier, "some_variable_name"),
            (EndOfFile, "\0"),
        });
    }
    
    [Fact]
    public void LexerMoveNext_GivenIncrementer_LexesIncrementerToken()
    {
        const string input = """
            foo++; bar--;
            """;

        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            (Identifier, "foo"),
            (Increment, "++"),
            (Semicolon, ";"),
            (Identifier, "bar"),
            (Decrement, "--"),
            (Semicolon, ";"),
            (EndOfFile, "\0"),
        });
    }
    
    [Fact]
    public void LexerMoveNext_GivenComparisonOperators_LexesComparisonOperatorTokens()
    {
        const string input = """
            foo > bar; foo < bar;
            true <= false; false >= true;
            123.456 == 654.321;
            """;

        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            // foo > bar;
            (Identifier, "foo"),
            (GreaterThan, ">"),
            (Identifier, "bar"),
            (Semicolon, ";"),
            // foo < bar;
            (Identifier, "foo"),
            (LessThan, "<"),
            (Identifier, "bar"),
            (Semicolon, ";"),
            // true <= false;
            (True, "true"),
            (LessThanEquals, "<="),
            (False, "false"),
            (Semicolon, ";"),
            // false >= true
            (False, "false"),
            (GreaterThanEquals, ">="),
            (True, "true"),
            (Semicolon, ";"),
            // 123.456 == 654.321;
            (TokenType.Double, "123.456"),
            (TokenType.Equals, "=="),
            (TokenType.Double, "654.321"),
            (Semicolon, ";"),
            (EndOfFile, "\0"),
        });
    }
    
    [Fact]
    public void LexerMoveNext_GivenArithmeticOperators_LexesArithmeticOperatorTokens()
    {
        const string input = """
            1 + 2;
            three - four;
            6 / 3;
            3.5 * 2.25;
            !true;
            """;

        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            (Integer, "1"),
            (Plus, "+"),
            (Integer, "2"),
            (Semicolon, ";"),
            (Identifier, "three"),
            (Minus, "-"),
            (Identifier, "four"),
            (Semicolon, ";"),
            (Integer, "6"),
            (Slash, "/"),
            (Integer, "3"),
            (Semicolon, ";"),
            (TokenType.Double, "3.5"),
            (Splat, "*"),
            (TokenType.Double, "2.25"),
            (Semicolon, ";"),
            (Bang, "!"),
            (True, "true"),
            (Semicolon, ";"),
            (EndOfFile, "\0"),
        });
    }
    
    [Fact]
    public void LexerMoveNext_GivenLiterals_LexesLiterals()
    {
        const string input = """
            123; 123.456;
            "This is a string"; "Hello, world!";
            true; false;
            null;
            """;

        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            (Integer, "123"),
            (Semicolon, ";"),
            (TokenType.Double, "123.456"),
            (Semicolon, ";"),
            (TokenType.String, "This is a string"),
            (Semicolon, ";"),
            (TokenType.String, "Hello, world!"),
            (Semicolon, ";"),
            (True, "true"),
            (Semicolon, ";"),
            (False, "false"),
            (Semicolon, ";"),
            (Null, "null"),
            (Semicolon, ";"),
            (EndOfFile, "\0"),
        });
    }
    
    [Fact]
    public void LexerMoveNext_GivenIllegals_LexesIllegalTokens()
    {
        const string input = """
            1234.5678.1234.5678;
            `~
            """;

        Lexer lexer = new(input);
        AssertTokenSequenceMatches(lexer, new()
        {
            (TokenType.Double, "1234.5678"),
            (Illegal, "."),
            (TokenType.Double, "1234.5678"),
            (Semicolon, ";"),
            (Illegal, "`"),
            (Illegal, "~"),
            (EndOfFile, "\0"),
        });
    }
    
    // todo
    // illegals!

    private void AssertTokenSequenceMatches(Lexer lexer, List<(TokenType Type, string Literal)> expected)
    {
        foreach (var (type, literal) in expected)
        {
            Token token = lexer.NextToken();
            Assert.Equal(type, token.Type);
            Assert.Equal(literal, token.Literal.ToString());
        }
    }
}