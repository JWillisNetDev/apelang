namespace Ape.Interpreter.Lexing;

public enum TokenType
{
    // Special symbols
    Illegal = 0,
    EndOfFile,
    
    // Identifiers and Literals
    Number,
    Identifier, 
    String,
    
    // Simple Operators
    Plus,
    Minus,
    Slash,
    Splat,
    Bang,
    
    // Assignment Operators
    Assign,
    Increment,
    Decrement,
    
    // Delimiters
    OpenParen,CloseParen,
    OpenBrace, CloseBrace,
    OpenBracket, CloseBracket,
    Comma,
    Semicolon,
    Colon,
    
    // Keywords 
    Function,
    Let,
    True,
    False,
    Null,
    If,
    Else,
    For,
    ForEach,
    Return,
    
    // Relational Operators
    Equals,
    NotEquals,
    GreaterThan, GreaterThanEquals,
    LessThan, LessThanEquals,
}