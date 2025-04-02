using System;
using System.Collections.Generic;

public class Parser
{
    private readonly string input;
    private int position;
    public readonly List<ParseError> errors = new();
    private bool hasCriticalError;

    public Parser(string input)
    {
        this.input = input.Trim();
        position = 0;
    }

    public void Parse()
    {
        ParseConst();
        if (!hasCriticalError) ParseVal();
        if (!hasCriticalError) ParseIdentifier();
        if (!hasCriticalError) ParseType();
        if (!hasCriticalError) ParseValue();
        
        CheckFinalSemicolon();

        PrintErrors();
    }
    private void CheckFinalSemicolon()
    {
        int lastCharPos = input.Length - 1;
        while (lastCharPos >= 0 && char.IsWhiteSpace(input[lastCharPos]))
        {
            lastCharPos--;
        }

        if (lastCharPos < 0 || input[lastCharPos] != ';')
        {
            AddError(lastCharPos >= 0 ? lastCharPos + 1 : 0, 
                "Ожидается завершающий символ ';'");
        }
    }

    private void ParseConst()
    {
        var startPos = position;
        if (!ExpectKeyword("const", ref startPos))
        {
            if (LookAhead("val"))
                AddError(0, "Ожидается ключевое слово 'const'");
            else
                AddError(startPos, "Ожидается 'const'");
            hasCriticalError = true;
            return;
        }

        position = startPos;
        if (!CheckSpaceAfter())
        {
            AddError(position, "Ожидается пробел после 'const'");
        }
    }

    private void ParseVal()
    {
        var startPos = position;
        if (!ExpectKeyword("val", ref startPos))
        {
            AddError(startPos, "Ожидается 'val'");
            hasCriticalError = true;
            return;
        }

        position = startPos;
        if (!CheckSpaceAfter())
        {
            AddError(position, "Ожидается пробел после 'val'");
        }
    }

    private void ParseIdentifier()
    {
        SkipWhitespace();
        var startPos = position;

        if (position >= input.Length || !char.IsLetter(input[position]))
        {
            AddError(startPos, "Идентификатор должен начинаться с буквы");
            hasCriticalError = true;
            return;
        }

        while (position < input.Length && (char.IsLetterOrDigit(input[position]) || input[position] == '_'))
        {
            position++;
        }

        // Проверка что идентификатор не пустой
        if (position == startPos)
        {
            AddError(startPos, "Отсутствует идентификатор");
            hasCriticalError = true;
        }
    }

    private void ParseType()
    {
        SkipWhitespace();
        var startPos = position;

        if (position >= input.Length || input[position] != ':')
        {
            AddError(startPos, "Ожидается символ ':'");
            hasCriticalError = true;
            return;
        }
        position++;

        SkipWhitespace();
        startPos = position;

        if (!ExpectKeyword("Double", ref position))
        {
            AddError(startPos, "Ожидается тип 'Double'");
            hasCriticalError = true;
        }
    }

    private void ParseValue()
    {
        SkipWhitespace();
        var startPos = position;

        if (position >= input.Length || input[position] != '=')
        {
            AddError(startPos, "Ожидается символ '='");
            hasCriticalError = true;
            return;
        }
        position++;

        SkipWhitespace();
        startPos = position;

        if (!ParseNumber())
        {
            AddError(startPos, "Ожидается числовое значение");
            hasCriticalError = true;
        }
    }

    private bool ParseNumber()
    {
        bool hasDigits = false;

        // Опциональный знак
        if (position < input.Length && (input[position] == '+' || input[position] == '-'))
        {
            position++;
        }

        // Целая часть
        while (position < input.Length && char.IsDigit(input[position]))
        {
            position++;
            hasDigits = true;
        }

        // Дробная часть
        if (position < input.Length && input[position] == '.')
        {
            position++;
            while (position < input.Length && char.IsDigit(input[position]))
            {
                position++;
                hasDigits = true;
            }
        }

        return hasDigits;
    }
    

    private bool ExpectKeyword(string keyword, ref int pos)
    {
        SkipWhitespace(ref pos);
        if (pos + keyword.Length > input.Length) return false;

        bool match = input.Substring(pos, keyword.Length).Equals(keyword);
        if (match) pos += keyword.Length;
        return match;
    }

    private bool CheckSpaceAfter()
    {
        if (position < input.Length && !char.IsWhiteSpace(input[position])) 
            return false;
        
        SkipWhitespace();
        return true;
    }

    private void SkipWhitespace()
    {
        while (position < input.Length && char.IsWhiteSpace(input[position]))
        {
            position++;
        }
    }

    private void SkipWhitespace(ref int pos)
    {
        while (pos < input.Length && char.IsWhiteSpace(input[pos]))
        {
            pos++;
        }
    }

    private bool LookAhead(string keyword)
    {
        return position + keyword.Length <= input.Length && 
               input.Substring(position, keyword.Length).Equals(keyword);
    }

    private void AddError(int errorPosition, string message)
    {
        errors.Add(new ParseError(errorPosition, message));
    }

    public void PrintErrors()
    {
        if (errors.Count == 0)
        {
            Console.WriteLine("Разбор завершен успешно!");
            return;
        }

        Console.WriteLine($"Найдено {errors.Count} ошибок:");
        foreach (var error in errors)
        {
            Console.WriteLine($"Позиция {error.Position}: {error.Message}");
        }
    }

    public record ParseError(int Position, string Message);
}