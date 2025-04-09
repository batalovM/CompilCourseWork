class Parser
{
    static int _pos;  
    static bool ParseKeyword(string _input, string expectedKeyword)
    {
        bool success = true;
        List<char> trashChars = new List<char>();
        int initialPos = _pos; 
        while (_pos < _input.Length && _input[_pos] != expectedKeyword[0])
        {
            if (_input[_pos] == 'v' || _input[_pos] == 'D' || _input[_pos] == '=')
            {
                Console.WriteLine($"Ожидалость ключевое слово {expectedKeyword}");
                return false;
            }
            trashChars.Add(_input[_pos]);
            _pos++;
        }

        if (trashChars.Count != 0)
        {
            Console.WriteLine($"Неизвестная конструкция в позиции {initialPos}: отброшенный фрагмент: {new string(trashChars.ToArray())}");
        }
        
        int wordStartPos = _pos;
        int expectedIndex = 0;
        bool alignmentDone = false;

        while (_pos < _input.Length && expectedIndex < expectedKeyword.Length)
        {
            char inputChar = _input[_pos];
            char expectedChar = expectedKeyword[expectedIndex];

            if (inputChar == expectedChar)
            {
                expectedIndex++;
                _pos++;
            }
            else
            {
                int lookaheadIndex = -1;
                if (expectedIndex + 1 < expectedKeyword.Length)
                {
                    lookaheadIndex = expectedKeyword.IndexOf(inputChar, expectedIndex + 1);
                }
                if (lookaheadIndex != -1)
                {
                    Console.WriteLine($"Ожидалось ключевое слово:'{expectedKeyword}'");
                    expectedIndex = lookaheadIndex;
                    alignmentDone = true; 
                    if (inputChar == expectedKeyword[expectedIndex])
                    {
                        expectedIndex++;
                        _pos++;
                    } else {
                         success = false; 
                         _pos++; 
                    }
                }
                else
                {
                    Console.WriteLine($"Ошибка: Неожиданный символ '{inputChar}' на позиции {_pos}");
                    success = false;
                    _pos++; 
                }
            }
        }
        if (expectedIndex < expectedKeyword.Length)
        {
            if (success && !alignmentDone)
            {
                 Console.WriteLine($"Ошибка: Ожидалось '{expectedKeyword}', но строка закончилась (проверка началась с {wordStartPos})");
            } else if (!success) {
                 Console.WriteLine($"Ошибка: Ключевое слово '{expectedKeyword}' не распознано полностью из-за ошибок (проверка началась с {wordStartPos})");
            } else if (alignmentDone) {
                 Console.WriteLine($"Ошибка: Ключевое слово '{expectedKeyword}' не завершено после выравнивания (проверка началась с {wordStartPos})");
            }

            success = false;
        }

        return success;
    }
    static bool ParseId(string _input)
    {
        bool success = true;
        List<char> trashChars = new List<char>();
        
        while (_pos < _input.Length && !(char.IsLetter(_input[_pos]) && _input[_pos] != '_'))
        {
            if (_input[_pos] == ':')
            {
                Console.WriteLine($"Ожидалась переменная");
                return false;
            }
            trashChars.Add(_input[_pos]);
            _pos++;
        }

        if (trashChars.Count != 0)
        {
            Console.WriteLine($"Неизвестная конструкция в позиции {_pos - trashChars.Count}: отброшенный фрагмент: {new string(trashChars.ToArray())}");
        }
        
        if (_pos < _input.Length && (char.IsLetter(_input[_pos]) || _input[_pos] == '_'))
        {
            while (_pos < _input.Length && (char.IsLetterOrDigit(_input[_pos]) || _input[_pos] == '_' || char.IsDigit(_input[_pos])))
            {
                _pos++;
            }
        }
        else
        {
            Console.WriteLine($"Ошибка: ожидался идентификатор (начинается с буквы или '_') на позиции {_pos}");
            success = false;
        }

        return success;
    }
    static bool ParseColon(string _input)
    {
        bool success = true;

        if (_pos < _input.Length && _input[_pos] == ':')
        {
            _pos++;
        }
        else
        {
            Console.WriteLine($"Ошибка: ожидалось двоеточие ':' на позиции {_pos}");
            success = false;
        }

        return success;
    }
    static bool ParseEqual(string _input)
    {
        bool success = true;

        if (_pos < _input.Length && _input[_pos] == '=')
        {
            _pos++;
        }
        else
        {
            Console.WriteLine($"Ошибка: ожидалось знак равенства '=' на позиции {_pos}");
            success = false;
        }

        return success;
    }
    static bool ParseNum(string _input)
    {
        bool success = true;
        List<char> trashChars = new List<char>();
        
        while (_pos < _input.Length && !char.IsDigit(_input[_pos]))
        {
            trashChars.Add(_input[_pos]);
            _pos++;
        }

        if (trashChars.Count != 0)
        {
            Console.WriteLine($"Неизвестная конструкция в позиции {_pos - trashChars.Count}: отброшенный фрагмент: {new string(trashChars.ToArray())}");
        }
        
        if (_pos < _input.Length && char.IsDigit(_input[_pos]))
        {
            while (_pos < _input.Length && char.IsDigit(_input[_pos]))
            {
                _pos++;
            }
        }
        else
        {
            Console.WriteLine($"Ошибка: ожидалось число на позиции {_pos}");
            success = false;
        }

        return success;
    }
    static bool ParseEnd(string _input)
    {
        bool success = true;

        if (_pos < _input.Length && _input[_pos] == ';')
        {
            _pos++;
        }
        else
        {
            Console.WriteLine($"Ошибка: ожидался завершающий символ ';' на позиции {_pos}");
            success = false;
        }

        return success;
    }
    static void Parse(string _input)
    {
        if (ParseKeyword(_input, "const"))
        {
            if (_input[_pos] != ' ')
            {
                Console.WriteLine($"Ошибка: ожидался пробел после 'const''");
            }
        }
        while (_pos < _input.Length && char.IsWhiteSpace(_input[_pos]))
        {
            _pos++;
        }
        
        if (ParseKeyword(_input, "val"))
        {
            if (_input[_pos] != ' ')
            {
                Console.WriteLine($"Ошибка: ожидался пробел после 'val''");
            }
        }
        
        while (_pos < _input.Length && char.IsWhiteSpace(_input[_pos]))
        {
            _pos++;
        }
        ParseId(_input);
        while (_pos < _input.Length && char.IsWhiteSpace(_input[_pos]))
        {
            _pos++;
        }
        ParseColon(_input);
        ParseKeyword(_input, "Double");
        ParseEqual(_input);
        while (_pos < _input.Length && char.IsWhiteSpace(_input[_pos]))
        {
            _pos++;
        }
        ParseNum(_input);
        ParseEnd(_input);
    
    }
    static void Main(string[] args)
    {
        Parse("const val pi:Double=123;");
    }
    //TODO ошибка на отсутствие ключевого слова
}

