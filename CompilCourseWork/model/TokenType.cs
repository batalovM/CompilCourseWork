using System;
using System.Collections.Generic;

namespace CompilCourseWork.model;

public enum TokenType
{
    EOF,
    ID,
    OPERATOR,
    LPAREN,
    RPAREN
} 
public struct Token
{
    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value = "")
    {
        Type = type;
        Value = value;
    }
}
 public class Lexer
{
    private readonly string _input;
    private int _position;
    private char _currentChar;

    public int GetPosition()
    {
        return _position;
    }
    public Lexer(string input)
    {
        _input = input;
        _position = 0;
        _currentChar = _input.Length > 0 ? _input[_position] : '\0';
    }

    private void Advance()
    {
        _position++;
        _currentChar = _position < _input.Length ? _input[_position] : '\0';
    }

    private void SkipWhitespace()
    {
        while (_currentChar != '\0' && char.IsWhiteSpace(_currentChar))
        {
            Advance();
        }
    }

    private string GetIdentifier()
    {
        var result = "";
        while (_currentChar != '\0' && char.IsLetter(_currentChar))
        {
            result += _currentChar;
            Advance();
        }
        return result;
    }

    public Token GetNextToken()
    {
        while (_currentChar != '\0')
        {
            if (char.IsWhiteSpace(_currentChar))
            {
                SkipWhitespace();
                continue;
            }

            if (char.IsLetter(_currentChar))
            {
                return new Token(TokenType.ID, GetIdentifier());
            }

            switch (_currentChar)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                    var op = _currentChar.ToString();
                    Advance();
                    return new Token(TokenType.OPERATOR, op);
                case '(':
                    Advance();
                    return new Token(TokenType.LPAREN);
                case ')':
                    Advance();
                    return new Token(TokenType.RPAREN);
                default:
                    throw new Exception($"Invalid character: {_currentChar}");
            }
        }

        return new Token(TokenType.EOF);
    }
}
public class Tetrad
{
    public string Op { get; }
    public string Arg1 { get; }
    public string Arg2 { get; }
    public string Result { get; }

    public Tetrad(string op, string arg1, string arg2, string result)
    {
        Op = op;
        Arg1 = arg1;
        Arg2 = arg2;
        Result = result;
    }

    public override string ToString()
    {
        return $"({Op}, {Arg1}, {Arg2}, {Result})";
    }
}
public class Parser
    {
        private readonly Lexer _lexer;
        private Token _currentToken;
        private int _tempCounter;
        public List<Tetrad> Tetrads { get; }

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            _currentToken = _lexer.GetNextToken();
            _tempCounter = 0;
            Tetrads = new List<Tetrad>();
        }

        private void Error(string message = "")
        {
            throw new Exception($"Syntax error: {message}. At position: {_lexer.GetPosition()}");
        }

        private void Eat(TokenType tokenType)
        {
            if (_currentToken.Type == tokenType)
            {
                _currentToken = _lexer.GetNextToken();
            }
            else
            {
                Error($"Expected {tokenType}, got {_currentToken.Type}");
            }
        }

        public string Parse()
        {
            var result = E();
            if (_currentToken.Type != TokenType.EOF)
            {
                Error("Unexpected token at end of expression");
            }
            return result;
        }

        private string E()
        {
            // E → T ( (+|-) T )*
            var left = T();
            while (_currentToken.Type == TokenType.OPERATOR && (_currentToken.Value == "+" || _currentToken.Value == "-"))
            {
                var op = _currentToken.Value;
                Eat(TokenType.OPERATOR);
                var right = T(); // T() уже обрабатывает */ с высшим приоритетом
                var temp = NewTemp();
                Emit(op, left, right, temp);
                left = temp;
            }
            return left;
        }


        private string A(string tempIn)
        {
            // Если следующий оператор + или -, сначала обрабатываем правую часть
            if (_currentToken.Type == TokenType.OPERATOR && (_currentToken.Value == "+" || _currentToken.Value == "-"))
            {
                var op = _currentToken.Value;
                Eat(TokenType.OPERATOR);
                var tempRight = T(); // Обрабатываем правую часть (с умножениями/делениями)
                var tempOut = NewTemp();
                Emit(op, tempIn, tempRight, tempOut);
                return A(tempOut);
            }

            return tempIn;
        }

        private string T()
        {
            // T → O ( (*|/) O )*
            var left = O();
            while (_currentToken.Type == TokenType.OPERATOR && (_currentToken.Value == "*" || _currentToken.Value == "/"))
            {
                var op = _currentToken.Value;
                Eat(TokenType.OPERATOR);
                var right = O();
                var temp = NewTemp();
                Emit(op, left, right, temp);
                left = temp;
            }
            return left;
        }

        private string B(string tempIn)
        {
            // В → ε | *ОВ | /ОВ
            if (_currentToken.Type == TokenType.OPERATOR && (_currentToken.Value == "*" || _currentToken.Value == "/"))
            {
                var op = _currentToken.Value;
                Eat(TokenType.OPERATOR);
                var temp1 = O();
                var tempOut = NewTemp();
                Emit(op, tempIn, temp1, tempOut);
                return B(tempOut);
            }
            return tempIn;
        }

        private string O()
        {
            // О → id | (E)
            if (_currentToken.Type == TokenType.ID)
            {
                var idName = _currentToken.Value;
                Eat(TokenType.ID);
                return idName;
            }
            else if (_currentToken.Type == TokenType.LPAREN)
            {
                Eat(TokenType.LPAREN);
                var temp = E();
                if (_currentToken.Type != TokenType.RPAREN)
                {
                    Error("Expected closing parenthesis");
                }
                Eat(TokenType.RPAREN);
                return temp;
            }
            else
            {
                Error("Expected identifier or '('");
                return null; // Этот код никогда не выполнится из-за выброса исключения
            }
        }

        private string NewTemp()
        {
            _tempCounter++;
            return $"t{_tempCounter}";
        }

        private void Emit(string op, string arg1, string arg2, string result)
        {
            Tetrads.Add(new Tetrad(op, arg1, arg2, result));
        }
    }
