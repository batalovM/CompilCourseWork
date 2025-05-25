using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CompilCourseWork.model;

public class ParseLaba8
{
        private readonly string input;
        private int pos;
        private readonly int length;
        private readonly List<string> errors;
        private readonly List<string> callStack;
        private int indentLevel;

        public ParseLaba8(string input)
        {
            this.input = input.Trim();
            this.pos = 0;
            this.length = this.input.Length;
            this.errors = new List<string>();
            this.callStack = new List<string>();
            this.indentLevel = 0;
        }

        private char? Peek()
        {
            return pos < length ? input[pos] : (char?)null;
        }

        private void Advance()
        {
            pos++;
        }

        private void SkipWhitespace()
        {
            while (pos < length && char.IsWhiteSpace(input[pos]))
            {
                pos++;
            }
        }

        private bool Match(string expected)
        {
            SkipWhitespace();
            if (pos + expected.Length <= length && input.Substring(pos, expected.Length) == expected)
            {
                pos += expected.Length;
                return true;
            }
            return false;
        }

        private void LogCall(string procedure)
        {
            Console.WriteLine(new string(' ', indentLevel * 2) + $"Вызов: {procedure}");
            callStack.Add(procedure);
            indentLevel++;
        }

        private void LogReturn()
        {
            indentLevel--;
            string procedure = callStack[callStack.Count - 1];
            callStack.RemoveAt(callStack.Count - 1);
            Console.WriteLine(new string(' ', indentLevel * 2) + $"Возврат из: {procedure}");
        }

        public bool Parse()
        {
            LogCall("Парсинг");
            Expression();
            SkipWhitespace();
            if (pos < length)
            {
                errors.Add($"Неожиданные символы в позиции {pos}: {input.Substring(pos)}");
            }
            LogReturn();
            if (errors.Count > 0)
            {
                Console.WriteLine("Обнаружены синтаксические ошибки:");
                foreach (var error in errors)
                {
                    Console.WriteLine($" - {error}");
                }
                return false;
            }
            return true;
        }

        private void Expression()
        {
            LogCall("Выражение");
            SimpleExpression();
            SkipWhitespace();
            if (Peek() == '=' || Peek() == '!' || Peek() == '<' || Peek() == '>')
            {
                int savedPos = pos;
                if (!RelationOp())
                {
                    pos = savedPos;
                    SimpleExpression();
                }
                else
                {
                    SimpleExpression();
                }
            }
            LogReturn();
        }

        private void SimpleExpression()
        {
            LogCall("Простое выражение");
            Term();
            while (true)
            {
                SkipWhitespace();
                int savedPos = pos;
                if (Match("+") || Match("-") || Match("or"))
                {
                    LogCall("Аддитивная операция");
                    LogReturn();
                    Term();
                }
                else
                {
                    pos = savedPos;
                    break;
                }
            }
            LogReturn();
        }

        private void Term()
        {
            LogCall("Терм");
            Factor();
            while (true)
            {
                SkipWhitespace();
                int savedPos = pos;
                if (Match("*") || Match("/") || Match("%"))
                {
                    LogCall("Мультипликативная операция");
                    LogReturn();
                    Factor();
                }
                else
                {
                    pos = savedPos;
                    break;
                }
            }
            LogReturn();
        }

        private void Factor()
        {
            LogCall("Фактор");
            SkipWhitespace();
            int savedPos = pos;
            if (Match("not"))
            {
                Factor();
            }
            else if (Match("("))
            {
                Expression();
                if (!Match(")"))
                {
                    errors.Add($"Ожидалась закрывающая скобка в позиции {pos}");
                    pos = savedPos;
                }
                else
                {
                    LogReturn();
                    return;
                }
            }
            else
            {
                Variable();
            }
            LogReturn();
        }

        private void Variable()
        {
            LogCall("Переменная");
            SkipWhitespace();
            int savedPos = pos;
            if (pos >= length || !char.IsLetter(input[pos]))
            {
                errors.Add($"Ожидалась буква в позиции {pos}");
                pos = savedPos + 1;
                LogReturn();
                return;
            }

            Regex pattern = new Regex(@"^[a-zA-Z][a-zA-Z0-9]*");
            Match match = pattern.Match(input.Substring(pos));
            if (!match.Success)
            {
                errors.Add($"Недопустимое имя переменной в позиции {pos}");
                pos = savedPos + 1;
                LogReturn();
                return;
            }

            pos += match.Value.Length;
            LogReturn();
        }

        private bool RelationOp()
        {
            LogCall("Операция отношения");
            SkipWhitespace();
            int savedPos = pos;
            if (Match("=") || Match("!=") || Match("<=") || Match(">=") || Match("<") || Match(">"))
            {
                LogReturn();
                return true;
            }
            errors.Add($"Ожидался оператор отношения в позиции {pos}");
            pos = savedPos;
            LogReturn();
            return false;
        }
}