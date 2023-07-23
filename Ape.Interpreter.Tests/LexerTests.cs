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
            (TokenType.Identifier, "foo"),
            (Assign, "="),
            (Number, "1.234"),
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
            (TokenType.Identifier, "foo"),
            (TokenType.Identifier, "bar"),
            (TokenType.Identifier, "test12345"),
            (TokenType.Identifier, "some_variable_name"),
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
            (TokenType.Identifier, "foo"),
            (Increment, "++"),
            (Semicolon, ";"),
            (TokenType.Identifier, "bar"),
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
            (TokenType.Identifier, "foo"),
            (GreaterThan, ">"),
            (TokenType.Identifier, "bar"),
            (Semicolon, ";"),
            // foo < bar;
            (TokenType.Identifier, "foo"),
            (LessThan, "<"),
            (TokenType.Identifier, "bar"),
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
            (Number, "123.456"),
            (TokenType.Equals, "=="),
            (Number, "654.321"),
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
            (Number, "1"),
            (Plus, "+"),
            (Number, "2"),
            (Semicolon, ";"),
            (TokenType.Identifier, "three"),
            (Minus, "-"),
            (TokenType.Identifier, "four"),
            (Semicolon, ";"),
            (Number, "6"),
            (Slash, "/"),
            (Number, "3"),
            (Semicolon, ";"),
            (Number, "3.5"),
            (Splat, "*"),
            (Number, "2.25"),
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
            (Number, "123"),
            (Semicolon, ";"),
            (Number, "123.456"),
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
            (Number, "1234.5678"),
            (Illegal, "."),
            (Number, "1234.5678"),
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