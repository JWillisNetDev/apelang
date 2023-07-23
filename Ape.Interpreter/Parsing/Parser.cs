using Ape.Interpreter.Lexing;
using Ape.Interpreter.Parsing.Ast;

namespace Ape.Interpreter.Parsing;

public class Parser
{
    private enum Precedence
    {
        Lowest = 0x00,
        Equals = 0x10,
        LessGreater = 0x20,
        Sum = 0x30,
        Product = 0x40,
        Prefix = 0x50,
        Call = 0x60,
        Index = 0x70,
    }

    private delegate Expression PrefixExpressionFactory();
    private delegate Expression InfixExpressionFactory(Expression left);
    
    public Lexer Lexer { get; }
    
    public Token Current { get; private set; } = null!;
    public Token Next { get; private set; } = null!;

    private readonly IReadOnlyDictionary<TokenType, PrefixExpressionFactory> _prefixParsers;
    private readonly IReadOnlyDictionary<TokenType, InfixExpressionFactory> _infixParsers;

    private static readonly IReadOnlyDictionary<TokenType, Precedence> PrecedenceLookup = new Dictionary<TokenType, Precedence>
    {
        [TokenType.Equals] = Precedence.Equals,
        [TokenType.NotEquals] = Precedence.Equals,
        [TokenType.LessThan] = Precedence.LessGreater,
        [TokenType.GreaterThan] = Precedence.LessGreater,
        [TokenType.Plus] = Precedence.Sum,
        [TokenType.Minus] = Precedence.Sum,
        [TokenType.Splat] = Precedence.Product,
        [TokenType.Slash] = Precedence.Product,
        [TokenType.OpenParen] = Precedence.Call,
        [TokenType.OpenBracket] = Precedence.Index,
    };

    // Exposed methods
    public Parser(Lexer lexer)
    {
        Lexer = lexer ?? throw new ArgumentNullException(nameof(lexer));
        NextToken();
        NextToken();

        _prefixParsers = new Dictionary<TokenType, PrefixExpressionFactory>
        {
            [TokenType.Identifier] = ParseIdentifier,
            [TokenType.Integer] = ParseNumber,
            [TokenType.Double] = ParseNumber,
        };

        // Register Infixes
    }

    public Program ParseProgram()
    {
        Program program = new();

        while (Current.Type != TokenType.EndOfFile)
        {
            program.AddStatement(ParseStatement());
            NextToken();
        }

        return program;
    }

    // Private static methods
    private static Precedence PrecedenceOf(TokenType type)
    {
        return PrecedenceLookup[type];
    }
    
    // Private methods
    private void NextToken()
    {
        Current = Next;
        Next = Lexer.NextToken();
    }

    private bool NextTokenMustBe(TokenType type)
    {
        if (NextTokenIs(type))
        {
            NextToken();
            return true;
        }
        return false;
    }

    private bool NextTokenIs(TokenType type)
    {
        return Next.Type == type;
    }

    private Expression ParseExpression(Precedence precedence)
    {
        if (!_prefixParsers.ContainsKey(Current.Type))
        {
            return null!;
        }
        PrefixExpressionFactory prefixParser = _prefixParsers[Current.Type];
        Expression left = prefixParser();

        while (!NextTokenIs(TokenType.Semicolon) && precedence < PrecedenceOf(Next.Type))
        {
            if (!_infixParsers.ContainsKey(Next.Type))
            {
                return null!;
            }
            InfixExpressionFactory infixParser = _infixParsers[Next.Type];
            NextToken();
            left = infixParser(left);
        }
        return left;
    }
    
    private IdentifierExpression ParseIdentifier()
    {
        return new IdentifierExpression(Current, Current.Literal);
    }

    private Expression ParseNumber() => Current.Type switch
    {
        TokenType.Integer => new IntegerLiteralExpression(Current, int.Parse(Current.Literal)),
        TokenType.Double => new DoubleLiteralExpression(Current, double.Parse(Current.Literal)),
        _ => throw new InvalidOperationException($"Attempted to parse non-number token {Current.Type} as number literal"),
    };

    private Statement ParseStatement()
    {
        return Current.Type switch
        {
            TokenType.Let => ParseLetStatement(),
            _ => ParseExpressionStatement(),
        };

        LetStatement ParseLetStatement()
        {
            Token token = Current;
            if (!NextTokenMustBe(TokenType.Identifier))
            {
                return null!;
            }
            IdentifierExpression name = new(Current, Current.Literal);

            if (NextTokenMustBe(TokenType.Assign))
            {
                return null!;
            }
            NextToken();

            Expression value = ParseExpression(Precedence.Lowest);

            return new LetStatement(token, name, value);
        }

        ExpressionStatement ParseExpressionStatement()
        {
            Token token = Current;
            Expression expression = ParseExpression(Precedence.Lowest);
            
            if (NextTokenIs(TokenType.Semicolon))
            {
                NextToken();
            }

            return new ExpressionStatement(token, expression);
        }
    }
}