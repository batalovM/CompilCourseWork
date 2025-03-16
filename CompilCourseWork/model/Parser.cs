using System;
using System.Collections.Generic;

class Parser
{
    private List<Token> tokens;
    private int position = 0;

    public Parser(List<Token> tokens)
    {
        this.tokens = tokens;
    }

    public void Parse()
    {
        if (ParseConstDeclaration() && position == tokens.Count)
        {
            Console.WriteLine("Синтаксический анализ завершен успешно!");
        }
        else
        {
            Console.WriteLine("Синтаксическая ошибка в позиции: " + (position < tokens.Count ? tokens[position].Position : "конец входных данных"));
        }
    }

    private bool ParseConstDeclaration()
    {
        return Match(14) && Match(14) && Match(2) && Match(15) && (Match(20) || Match(21) || Match(22)) && Match(10) && ParseExpression() && Match(16);
    }

    private bool ParseExpression()
    {
        return Match(3); // Только вещественное число
    }

    private bool Match(int expectedCode)
    {
        if (position >= tokens.Count)
        {
            Console.WriteLine($"Ошибка: достигнут конец входных данных, ожидался токен с кодом {expectedCode}");
            return false;
        }

        if (tokens[position].Code == expectedCode)
        {
            position++;
            return true;
        }
        return false;
    }
}
