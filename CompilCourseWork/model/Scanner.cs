// using System.Collections.Generic;
//
// public class Scanner
// {
//     private readonly string _input;
//     private int _position;
//     private readonly List<Token> _tokens = new List<Token>();
//
//     public Scanner(string input) => _input = input;
//
//     public List<Token> Analyze()
//     {
//         while (_position < _input.Length)
//         {
//             var currentChar = _input[_position];
//
//             if (char.IsWhiteSpace(currentChar))
//             {
//                 _position++;
//                 continue;
//             }
//
//             if (char.IsLetter(currentChar))
//                 ProcessIdentifierOrKeyword();
//             else if (char.IsDigit(currentChar))
//                 ProcessNumber();
//             else
//                 ProcessOperatorOrSymbol();
//         }
//         return _tokens;
//     }
//
//     private void ProcessIdentifierOrKeyword()
//     {
//         var start = _position;
//         while (_position < _input.Length && (char.IsLetterOrDigit(_input[_position]) || _input[_position] == '_'))
//             _position++;
//
//         var lexeme = _input.Substring(start, _position - start);
//         var type = lexeme switch
//         {
//             "const" or "val" or "Double" => "ключевое слово",
//             _ => "идентификатор"
//         };
//         var code = type == "ключевое слово" ? 14 : 2;
//
//         AddToken(code, type, lexeme);
//     }
//
//     private void ProcessNumber()
//     {
//         var start = _position;
//         var isDouble = false;
//
//         while (_position < _input.Length && char.IsDigit(_input[_position]))
//             _position++;
//
//         if (_position < _input.Length && _input[_position] == '.')
//         {
//             isDouble = true;
//             _position++;
//             while (_position < _input.Length && char.IsDigit(_input[_position]))
//                 _position++;
//         }
//
//         var number = _input.Substring(start, _position - start);
//         AddToken(isDouble ? 3 : 1, isDouble ? "вещественное число" : "целое число", number);
//     }
//
//     private void ProcessOperatorOrSymbol()
//     {
//         var currentChar = _input[_position];
//         switch (currentChar)
//         {
//             case '=':
//                 AddToken(10, "оператор присваивания", "=");
//                 break;
//             case ':':
//                 AddToken(15, "двоеточие", ":");
//                 break;
//             case ';':
//                 AddToken(16, "конец оператора", ";");
//                 break;
//             default:
//                 AddToken(-1, "недопустимый символ", currentChar.ToString());
//                 break;
//         }
//         _position++;
//     }
//
//     private void AddToken(int code, string type, string lexeme)
//     {
//         _tokens.Add(new Token
//         {
//             Code = code,
//             Type = type,
//             Lexeme = lexeme,
//             Position = $"{_position - lexeme.Length + 1} по {_position}"
//         });
//     }
// }