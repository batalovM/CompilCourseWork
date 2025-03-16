using System.Collections.Generic;

class Scanner
{
    private string input;
    private int position = 0;
    private List<Token> tokens = new List<Token>();

    public Scanner(string input)
    {
        this.input = input;
    }

    public List<Token> Analyze()
    {
        while (position < input.Length)
        {
            char currentChar = input[position];

            if (char.IsWhiteSpace(currentChar))
            {
                position++;
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
            else if (currentChar == '=' || currentChar == '+' || currentChar == '-')
            {
                ProcessOperator();
            }
            else if (currentChar == ':')
            {
                AddToken(15, "двоеточие", ":");
                position++;
            }
            else if (currentChar == ';')
            {
                AddToken(16, "конец оператора", ";");
                position++;
            }
            else
            {
                AddToken(-1, "недопустимый символ", currentChar.ToString());
                position++;
            }
        }
        return tokens;
    }

    private void ProcessIdentifierOrKeyword()
    {
        int start = position;
        while (position < input.Length && (char.IsLetterOrDigit(input[position]) || input[position] == '_'))
        {
            position++;
        }
        string lexeme = input.Substring(start, position - start);
        
        switch (lexeme)
        {
            case "int":
            case "const":
            case "val":
            case "Double":
                AddToken(14, "ключевое слово", lexeme);
                break;
            default:
                AddToken(2, "идентификатор", lexeme);
                break;
        }
    }

    private void ProcessNumber()
    {
        int start = position;
        bool isDouble = false;

        while (position < input.Length && char.IsDigit(input[position]))
        {
            position++;
        }

        if (position < input.Length && input[position] == '.')
        {
            isDouble = true;
            position++;
            while (position < input.Length && char.IsDigit(input[position]))
            {
                position++;
            }
        }

        string number = input.Substring(start, position - start);
        AddToken(isDouble ? 3 : 1, isDouble ? "вещественное число (Double)" : "целое без знака", number);
    }

    private void ProcessOperator()
    {
        char currentChar = input[position];
        int code = currentChar == '=' ? 10 : 4;
        string type = currentChar == '=' ? "оператор присваивания" : "оператор";

        AddToken(code, type, currentChar.ToString());
        position++;
    }

    private void AddToken(int code, string type, string lexeme)
    {
        tokens.Add(new Token { Code = code, Type = type, Lexeme = lexeme, Position = $"{position - lexeme.Length + 1} по {position}" });
    }
}