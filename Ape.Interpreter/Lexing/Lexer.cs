namespace Ape.Interpreter.Lexing;

public class Lexer
{
    // Private fields
    private int _position;
    private int _readPosition;
    private char _current;

    // Public readonly properties
    public string Input { get; }
    
    public Lexer(string input)
    {
        Input = input;
        ReadChar();
    }

    // Private static methods
    private static bool IsDigit(char c)
    {
        return char.IsDigit(c);
    }
    
    private static bool IsLetterOrUnderscore(char c)
    {
        return char.IsLetter(c) || c == '_';
    }
    
    // Public instance methods
    public Token NextToken()
    {
        SkipWhiteSpace();
        if (IsLetterOrUnderscore(_current))
        {
            return NextIdentifier();
        }
        
        if (IsDigit(_current))
        {
            return NextNumber();
        }
        
        return NextOperator();

    }

    private Token NextIdentifier()
    {
        string literal = ReadIdentifier();
        TokenType type = literal switch
        {
            "fn" => TokenType.Function,
            "let" => TokenType.Let,
            "true" => TokenType.True,
            "false" => TokenType.False,
            "null" => TokenType.Null,
            "if" => TokenType.If,
            "else" => TokenType.Else,
            "for" => TokenType.For,
            "foreach" => TokenType.ForEach,
            "return" => TokenType.Return,
            _ => TokenType.Identifier,
        };
        return new Token(type, literal);
    }
    
    private Token NextNumber()
    {
        // Read all numbers to the first decimal, then read additional numbers if they exist.
        string number = ReadNumber();
        if (_current == '.')
        {
            ReadChar();
            string decimalNumber = ReadNumber();
            return new Token(TokenType.Double, $"{number}.{decimalNumber}");
        }
        return new Token(TokenType.Integer, number);
    }

    private Token NextOperator()
    {
        Token token;
        int startIndex = _position;
        switch (_current)
        {
            case '"':
                token = NextString();
                break;
            
            case '=':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.Equals, Input[startIndex.._readPosition]);
                    break;
                }
                token = new Token(TokenType.Assign, _current.ToString());
                break;
            
            case '!':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.NotEquals, Input[startIndex.._readPosition]);
                    break;
                }
                token = new Token(TokenType.Bang, _current.ToString());
                break;
            
            case '>':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.GreaterThanEquals, Input[startIndex.._readPosition]);
                    break;
                }
                token = new Token(TokenType.GreaterThan, _current.ToString());
                break;
            
            case '<':
                if (PeekChar() == '=')
                {
                    ReadChar();
                    token = new Token(TokenType.LessThanEquals, Input[startIndex.._readPosition]);
                    break;
                }
                token = new Token(TokenType.LessThan, _current.ToString());
                break;
            
            case '+':
                if (PeekChar() == '+')
                {
                    ReadChar();
                    token = new Token(TokenType.Increment, Input[startIndex.._readPosition]);
                    break;
                }
                token = new Token(TokenType.Plus, Input[startIndex.._readPosition]);
                break;
            
            case '-':
                if (PeekChar() == '-')
                {
                    ReadChar();
                    token = new Token(TokenType.Decrement, Input[startIndex.._readPosition]);
                    break;
                }
                token = new Token(TokenType.Minus, _current.ToString());
                break;
            
            case '\0':
                token = new Token(TokenType.EndOfFile, "\0");
                break;
            
            default:
                token = new Token(GetCharOperatorType(), _current.ToString());
                break;
        }
        
        ReadChar();
        return token;

        TokenType GetCharOperatorType() => _current switch
        {
            '/' => TokenType.Slash,
            '*' => TokenType.Splat,
            '(' => TokenType.OpenParen,
            ')' => TokenType.CloseParen,
            '{' => TokenType.OpenBrace,
            '}' => TokenType.CloseBrace,
            '[' => TokenType.OpenBracket,
            ']' => TokenType.CloseBracket,
            ',' => TokenType.Comma,
            ';' => TokenType.Semicolon,
            ':' => TokenType.Colon,
            _ => TokenType.Illegal,
        };
    }
    
    private Token NextString()
    {
        string literal = ReadString();
        return new Token(TokenType.String, literal);
    }

    private char PeekChar()
    {
        return _readPosition < Input.Length ? Input[_readPosition] : '\0';
    }

    private void ReadChar()
    {
        _current = _readPosition < Input.Length ? Input[_readPosition] : '\0';
        _position = _readPosition++;
    }
    
    private string ReadIdentifier()
    {
        int startIndex = _position;
        while (IsLetterOrUnderscore(_current) || IsDigit(_current))
        {
            ReadChar();
        }

        return Input[startIndex.._position];
    }

    private string ReadNumber()
    {
        int startIndex = _position;
        while (IsDigit(_current))
        {
            ReadChar();
        }
        return Input[startIndex.._position];
    }

    private string ReadString()
    {
        ReadChar();
        int startIndex = _position;
        while (_current is not ('"' or '\0'))
        {
            ReadChar();
        }
        return Input[startIndex.._position];
    }

    private void SkipWhiteSpace()
    {
        while (char.IsWhiteSpace(_current))
        {
            ReadChar();
        }
    }
}