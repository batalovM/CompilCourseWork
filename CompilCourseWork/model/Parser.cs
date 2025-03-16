using System;
using System.Collections.Generic;

class Parser
{
    private readonly List<Token> _tokens;
    private int _position;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
    }

    public bool Parse()
    {
        if (ParseConstDeclaration() && _position == _tokens.Count)
        {
            Console.WriteLine("Синтаксический анализ завершен успешно!");
            return true;
        }
        else
        {
            Console.WriteLine("Синтаксическая ошибка в позиции: " + (_position < _tokens.Count ? _tokens[_position].Position : "конец входных данных"));
            return false;
        }
    }

    private bool ParseConstDeclaration()
    {
        return Match(14) && 
               Match(14) && 
               Match(2) &&  
               Match(15) && 
               Match(14) && 
               Match(10) && 
               ParseExpression() && 
               Match(16);   
    }

    private bool ParseExpression()
    {
        return Match(3); 
    }

    private bool Match(int expectedCode)
    {
        if (_position >= _tokens.Count)
        {
            Console.WriteLine($"Ошибка: достигнут конец входных данных, ожидался токен с кодом {expectedCode}");
            return false;
        }

        if (_tokens[_position].Code == expectedCode)
        {
            _position++;
            return true;
        }
        return false;
    }
}