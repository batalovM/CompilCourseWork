// using System.Collections.Generic;
// namespace CompilCourseWork.model;
// public class Parser
// {
//     private int _pos;
//     private readonly List<string> _outputLog = new();
//     private bool ParseValKeyword(string input)
//     {
//         int initialPos = _pos;
//         bool success = true;
//         if (_pos + 2 < input.Length && input.Substring(_pos, 3) == "val")
//         {
//             _pos += 3;
//             return true;
//         }
//         if (_pos < input.Length)
//         {
//             if (input[_pos] == 'v')
//             {
//                 _pos++;
//                 if (_pos < input.Length && input[_pos] == 'a')
//                 {
//                     _pos++;
//                     if (_pos < input.Length && input[_pos] == 'l')
//                     {
//                         _pos++;
//                         return true;
//                     }
//                     else
//                     {
//                         _outputLog.Add($"Ошибка: Ожидалось ключевое слово 'val' на позиции {_pos}");
//                         success = false;
//                     }
//                 }
//                 else
//                 {
//                     _outputLog.Add($"Ошибка: Ожидалось ключевое слово 'val' на позиции {_pos}");
//                     success = false;
//                 }
//             }
//             else
//             {
//                 _outputLog.Add($"Ошибка: ожидалось ключевое слово 'val' на позиции {initialPos}");
//                 success = false;
//             }
//             while (_pos < input.Length && !char.IsWhiteSpace(input[_pos]))
//             {
//                 _pos++;
//             }
//         }
//         else
//         {
//             _outputLog.Add("Ошибка: ожидалось ключевое слово 'val', но строка закончилась");
//             success = false;
//         }
//
//         return success;
//     }
//     private bool ParseDoubleKeyword(string _input) 
//     {
//         bool success = true;
//         while (_pos < _input.Length && char.IsWhiteSpace(_input[_pos]))
//         {
//             _pos++;
//         }
//         if (_pos < _input.Length && _input[_pos] == ':')
//         {
//             _pos++;
//         }
//         while (_pos < _input.Length && char.IsWhiteSpace(_input[_pos]))
//         {
//             _pos++;
//         }
//         string expectedWord = "Double";
//         bool isDoubleCorrect = true;
//
//         for (int i = 0; i < expectedWord.Length; i++)
//         {
//             if (_pos + i >= _input.Length || _input[_pos + i] != expectedWord[i])
//             {
//                 isDoubleCorrect = false;
//                 break;
//             }
//         }
//
//         if (isDoubleCorrect)
//         {
//             _pos += expectedWord.Length;
//         }
//         else
//         {
//             _outputLog.Add($"Ошибка: ожидалось 'Double' на позиции {_pos}'");
//             success = false;
//             while (_pos < _input.Length && !char.IsWhiteSpace(_input[_pos]) && _input[_pos] != '=')
//             {
//                 _pos++;
//             }
//         }
//         return success;
//     }
//     
//     public List<string> Parse(string input)
//     {
//         _pos = 0;
//         _outputLog.Clear();
//         if (ParseKeyword(input, "const"))
//         {
//             while ( input[_pos] != ' ')
//             {
//                 if (input[_pos] == 'v' || input[_pos] == 'a')
//                 {
//                     _outputLog.Add($"Ошибка: ожидался пробел после 'const");
//                     break;
//                 }
//                 _outputLog.Add($"Ошибка: Неизвестная конструкция в позиции {_pos}: отброшенный фрагмент: {input[_pos]}");
//                 _pos++;
//             }   
//         }
//         SkipWhiteSpaces(input);
//         ParseValKeyword(input);
//         SkipWhiteSpaces(input);
//         ParseId(input);
//         SkipWhiteSpaces(input);
//         ParseColon(input);
//         SkipWhiteSpaces(input);
//         if (ParseDoubleKeyword(input))
//         {
//             while (input[_pos] != ' ')
//             {
//                 if (input[_pos] == '='){break;}
//                 _outputLog.Add($"Ошибка: неизвестный символ {input[_pos]} на позиции: {_pos}");
//                 _pos++;
//             }
//         }
//         SkipWhiteSpaces(input);
//         ParseEqual(input);
//         SkipWhiteSpaces(input);
//         ParseNum(input);
//         SkipWhiteSpaces(input);
//         ParseEnd(input);
//         return _outputLog;
//     }
//     private bool ParseKeyword(string input, string expectedKeyword)
//     {
//         bool success = true;
//         List<char> trashChars = new List<char>();
//         int initialPos = _pos;
//         while (_pos < input.Length && input[_pos] != expectedKeyword[0])
//         {
//             if (input[_pos] == 'v' || input[_pos] == 'D' || input[_pos] == '=')
//             {
//                 _outputLog.Add($"Ожидалось ключевое слово {expectedKeyword}");
//                 return false;
//             }
//
//             if (expectedKeyword == "val" && input[_pos] == 'a')
//             {
//                 break;
//             }
//             if (input[_pos] == ':')
//             {
//                 _outputLog.Add($"Ожидалась переменная");
//                 return false;
//             }
//             if (input[_pos] == 'D')
//             {
//                 _outputLog.Add($"Ожидалась ':'");
//                 return false;
//             }
//             trashChars.Add(input[_pos]);
//             _pos++;
//         }
//         if (trashChars.Count != 0)
//         {
//             _outputLog.Add($"Ошибка: Неизвестная конструкция в позиции {initialPos}: отброшенный фрагмент: {new string(trashChars.ToArray())}");
//         }
//         int wordStartPos = _pos;
//         int expectedIndex = 0;
//         bool alignmentDone = false;
//         while (_pos < input.Length && expectedIndex < expectedKeyword.Length)
//         {
//             char inputChar = input[_pos];
//             char expectedChar = expectedKeyword[expectedIndex];
//             if (expectedKeyword == "const")
//             {
//                 if (input[_pos] == ' ')
//                 {
//                     _outputLog.Add($"Ошибка: Ожидалось ключевое слово {expectedKeyword}");
//                     return false;
//                 }
//                 if (input[_pos] == 'v')
//                 {
//                     _outputLog.Add($"Ошибка: Ожидалось ключевое слово {expectedKeyword} на позиции {_pos}");
//                     _outputLog.Add($"Ошибка: Ожидался пробел после 'const'");
//                     return false;
//                 }
//             }
//             if (expectedKeyword == "val")
//             {
//                 if (input[_pos] == ' ')
//                 {
//                     _outputLog.Add($"Ошибка: Ожидалось ключевое слово {expectedKeyword}");
//                     return false;
//                 }
//                 if (input[_pos] == '_' || 
//                     (char.IsLetter(input[_pos]) && input[_pos] != 'v' && input[_pos] != 'a' && input[_pos] != 'l') || 
//                     input[_pos] == ' ')
//                 {
//                     _outputLog.Add($"Ошибка: Ожидалось ключевое слово {expectedKeyword}");
//                     _outputLog.Add($"Ошибка: Ожидался пробел после 'val'");
//                     return false;
//                 }
//             }
//             if (inputChar == expectedChar)
//             {
//                 expectedIndex++;
//                 _pos++;
//             }
//             else
//             {
//                 int lookaheadIndex = -1;
//                 if (expectedIndex + 1 < expectedKeyword.Length)
//                 {
//                     lookaheadIndex = expectedKeyword.IndexOf(inputChar, expectedIndex + 1);
//                 }
//                 if (lookaheadIndex != -1)
//                 {
//                     _outputLog.Add($"Ошибка: Ожидалось ключевое слово:'{expectedKeyword}' на позиции {_pos}");
//                     expectedIndex = lookaheadIndex;
//                     alignmentDone = true;
//                     if (inputChar == expectedKeyword[expectedIndex])
//                     {
//                         expectedIndex++;
//                         _pos++;
//                     }
//                     else
//                     {
//                         success = false;
//                         _pos++;
//                     }
//                 }
//                 else
//                 {
//                     _outputLog.Add($"Ошибка: Неожиданный символ '{inputChar}' на позиции {_pos}");
//                     success = false;
//                     _pos++;
//                 }
//             }
//         }
//         return success;
//     }
//     private bool ParseId(string input)
//     {
//         bool success = true;
//         List<char> trashChars = new List<char>();
//         while (_pos < input.Length && !(char.IsLetter(input[_pos]) && input[_pos] != '_'))
//         {
//             if (input[_pos] == ':')//TODO
//             {
//                 _outputLog.Add($"Ожидалась переменная");
//                 return false;
//             }
//             if (input[_pos] == 'D')//TODO
//             {
//                 _outputLog.Add($"Ожидалась переменная");
//                 return false;
//             }
//             trashChars.Add(input[_pos]);
//             _pos++;
//         }
//         if (trashChars.Count != 0)
//         {
//             _outputLog.Add($"Ошибка: Неизвестная конструкция в позиции {_pos - trashChars.Count}: отброшенный фрагмент: {new string(trashChars.ToArray())}");
//         }
//         if (_pos < input.Length && (char.IsLetter(input[_pos]) || input[_pos] == '_'))
//         {
//             
//             doKall(input);
//             if ("!@#$%^&*()".Contains(input[_pos]))
//             {
//                 _outputLog.Add($"Ошибка: Недопустимый символ '{input[_pos]}' внутри идентификатора (позиция {_pos})");
//                 _pos++;
//                 doKall(input);
//             }
//         }
//         else
//         {
//             _outputLog.Add($"Ошибка: ожидался идентификатор (начинается с буквы или '_') на позиции {_pos}");
//             success = false;
//         }
//         return success;
//     }
//     private void doKall(string input) 
//     {while (_pos < input.Length && (char.IsLetterOrDigit(input[_pos]) || input[_pos] == '_' || char.IsDigit(input[_pos])))
//         {
//             _pos++;
//         } }
//     private bool ParseColon(string input) 
//     {
//         bool success = true;
//         if (_pos < input.Length && input[_pos] == ':')
//         {
//             _pos++;
//             if (_pos < input.Length && input[_pos] == ':')
//             {
//                 _outputLog.Add($"Ошибка: обнаружено {input[_pos]} на позиции {_pos}");
//                 _pos++;
//                 success = false;
//             }
//         }
//         else
//         {
//             _outputLog.Add($"Ошибка: ожидалось двоеточие ':' на позиции {_pos}");
//             success = false;
//         }
//         return success;
//     }
//     private bool ParseEqual(string _input)
//     {
//         bool success = true;
//         if (_pos < _input.Length && _input[_pos] == '=')
//         {
//             _pos++;
//             if (_pos < _input.Length && _input[_pos] == '=')
//             {
//                 _outputLog.Add($"Ошибка: обнаружено {_input[_pos]} на позиции {_pos}");
//                 _pos++;
//                 success = false;
//             }
//         }
//         else
//         {
//             _outputLog.Add($"Ошибка: ожидалось знак равенства '=' на позиции {_pos}");
//             success = false;
//         }
//         return success;
//     }
//     private bool ParseNum(string _input)
//     {
//         bool success = true;
//         List<char> trashChars = new List<char>();
//         while (_pos < _input.Length && !char.IsDigit(_input[_pos]))
//         {
//             trashChars.Add(_input[_pos]);
//             _pos++;
//         }
//         if (trashChars.Count != 0)
//         {
//             _outputLog.Add($"Ошибка: Неизвестная конструкция в позиции {_pos - trashChars.Count}: отброшенный фрагмент: {new string(trashChars.ToArray())}");
//         }
//         int dotCount = 0;
//         if (_pos < _input.Length && char.IsDigit(_input[_pos]))
//         {
//             while (_pos < _input.Length)
//             {
//                 if (char.IsDigit(_input[_pos]))
//                 {
//                     _pos++;
//                 }
//                 else if (_input[_pos] == '.')
//                 {
//                     dotCount++;
//                     if (dotCount > 1)
//                     {
//                         _outputLog.Add($"Ошибка: лишняя точка в позиции {_pos}");
//                         success = false;
//                     }
//                     _pos++;
//                 }
//                 else if (_input[_pos] == ';')
//                 {
//                     break;
//                 }
//                 else
//                 {
//                     _outputLog.Add($"Ошибка: неожиданный символ '{_input[_pos]}' на позиции {_pos}");
//                     _pos++;
//                     success = false;
//                 }
//             }
//         }
//         else
//         {
//             _outputLog.Add($"Ошибка: ожидалось число на позиции {_pos}");
//             success = false;
//         }
//         return success;
//     }
//
//     private bool ParseEnd(string input)
//     {
//         bool success = true;
//         if (_pos < input.Length && input[_pos] == ';')
//         {
//             _pos++;
//             if (_pos < input.Length)
//             {
//                 _outputLog.Add($"Ошибка: лишние символы после ';' (позиция {_pos})");
//                 success = false;
//             }
//         }
//         else
//         {
//             _outputLog.Add($"Ошибка: ожидался завершающий символ ';' на позиции {_pos}");
//             success = false;
//         }
//         return success;
//     }
//     private void SkipWhiteSpaces(string input)
//     {
//         int spaceCount = 0;
//         while (_pos < input.Length && char.IsWhiteSpace(input[_pos]) && spaceCount < 1)
//         {
//             spaceCount++;
//             _pos++;
//         }
//     }
// }
