using System.Collections.Generic;

namespace CompilCourseWork.model;

class Scanner
{
    private readonly string _input;
    private int _position;
    private readonly List<Token> _tokens = new();

    public Scanner(string input)
    {
        _input = input;
    }

    public List<Token> Analyze()
    {
        while (_position < _input.Length)
        {
            var currentChar = _input[_position];

            if (char.IsWhiteSpace(currentChar))
            {
                _position++;
                continue;
            }

            if (char.IsLetter(currentChar))
            {
                ProcessIdentifierOrKeyword();
            }
            else if (char.IsDigit(currentChar))
            {
                ProcessNumber();
            }
            else switch (currentChar)
            {
                case '=':
                    ProcessOperator();
                    break;
                case ':':
                    AddToken(15, "двоеточие", ":");
                    _position++;
                    break;
                case ';':
                    AddToken(16, "конец оператора", ";");
                    _position++;
                    break;
                default:
                    AddToken(-1, "недопустимый символ", currentChar.ToString());
                    _position++;
                    break;
            }
        }
        return _tokens;
    }

    private void ProcessIdentifierOrKeyword()
    {
        var start = _position;
        while (_position < _input.Length && (char.IsLetterOrDigit(_input[_position]) || _input[_position] == '_'))
        {
            _position++;
        }
        var lexeme = _input.Substring(start, _position - start);

        switch (lexeme)
        {
            case "int":
            case "const":
            case "val":
            case "Double":
            case "Float":
            case "Int":
                AddToken(14, "ключевое слово", lexeme);
                break;
            default:
                AddToken(2, "идентификатор", lexeme);
                break;
        }
    }

    private void ProcessNumber()
    {
        var start = _position;
        var isDouble = false;

        while (_position < _input.Length && char.IsDigit(_input[_position]))
        {
            _position++;
        }

        if (_position < _input.Length && _input[_position] == '.')
        {
            isDouble = true;
            _position++;
            while (_position < _input.Length && char.IsDigit(_input[_position]))
            {
                _position++;
            }
        }

        var number = _input.Substring(start, _position - start);
        AddToken(isDouble ? 3 : 1, isDouble ? "вещественное число (Double)" : "целое без знака", number);
    }

    private void ProcessOperator()
    {
        var currentChar = _input[_position];
        AddToken(10, "оператор присваивания", currentChar.ToString());
        _position++;
    }

    private void AddToken(int code, string type, string lexeme)
    {
        _tokens.Add(new Token { Code = code, Type = type, Lexeme = lexeme, Position = $"{_position - lexeme.Length + 1} по {_position}" });
    }
}