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
        if (ParseConstDeclaration())
        {
            Console.WriteLine("Синтаксический анализ завершен успешно!");
        }
        else
        {
            Console.WriteLine("Синтаксическая ошибка в позиции: " + tokens[position].Position);
        }
    }

    private bool ParseConstDeclaration()
    {
        return Match(14) && Match(14) && Match(2) && Match(15) && Match(14) && Match(10) && ParseExpression() && Match(16);
    }

    private bool ParseExpression()
    {
        return Match(1) || Match(3);
    }

    private bool Match(int expectedCode)
    {
        if (position < tokens.Count && tokens[position].Code == expectedCode)
        {
            position++;
            return true;
        }
        return false;
    }
}