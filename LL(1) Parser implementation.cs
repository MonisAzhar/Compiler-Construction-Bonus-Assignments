using System;
using System.Collections.Generic;

namespace BonusAssignmnet
{
    class Program
    {
        private static readonly Stack<string> Stack = new Stack<string>();
        private static readonly Queue<string> Input = new Queue<string>();
        private static string[,] _parsingTable;
        private static Dictionary<string, int> _columnsDictionary, _rowsDictionary;
        private static int _row = 0, _col = 0;

        static void Main(string[] args)
        {
            InitializeRowsColumnsDictionary();

            while (true)
            {
                Console.Write("Input: ");
                var raw = Console.ReadLine();

                ParseInput(raw);
                Console.WriteLine();
                ClearStacksAndQueue();
            }
        }

        private static void ClearStacksAndQueue()
        {
            while (Stack.Count != 0) Stack.Pop();
            while (Input.Count != 0) Input.Dequeue();
        }

        private static void ParseInput(string raw)
        {
            foreach (var t in GetTokens(raw))
                Input.Enqueue(t);

            Input.Enqueue("$");

            _parsingTable = GetParsingTable();

            Stack.Push("$");
            Stack.Push("E");

            while (true)
            {
                if (Input.Peek() == Stack.Peek() && Input.Peek() == "$")
                {
                    Console.WriteLine("Accepted");
                    break;
                }

                if (Input.Peek() == Stack.Peek())
                {
                    Stack.Pop();
                    Input.Dequeue();
                }

                _row = _rowsDictionary[Stack.Peek()];
                _col = _columnsDictionary[Input.Peek()];

                var data = _parsingTable[_row, _col];

                if (data == "error")
                {
                    Console.WriteLine("Syntax error");
                    break;
                }
                var body = data.Split('>')[1];
                if (body == "null")
                {
                    Stack.Pop();
                    continue;
                }

                Stack.Pop();

                foreach (var token in GetBodyTokens(body))
                    Stack.Push(token);
            }
        }

        private static IEnumerable<string> GetBodyTokens(string body)
        {
            if (body == "null" || body == "id")
                yield return body;
            else if (body == "TE'")
            {
                yield return "E'";
                yield return "T";
            }
            else if (body == "FT'")
            {
                yield return "T'";
                yield return "F";
            }
            else if (body == "(E)")
            {
                yield return ")";
                yield return "E";
                yield return "(";
            }
            else if (body == "+TE'")
            {
                yield return "E'";
                yield return "T";
                yield return "+";
            }
            else if (body == "*FT'")
            {
                yield return "T'";
                yield return "F";
                yield return "*";
            }
        }

        private static IEnumerable<dynamic> GetTokens(string raw)
        {
            for (int i = 0; i < raw.Length; i++)
            {
                if (raw[i] == '(' || raw[i] == ')' || raw[i] == '*' || raw[i] == '+')
                    yield return $"{raw[i]}";
                else if (raw[i] == 'i')
                {
                    if (raw[i + 1] == 'd')
                        yield return "id";
                    i++;
                }
            }
        }

        private static void InitializeRowsColumnsDictionary()
        {
            _columnsDictionary =
                new Dictionary<string, int> { { "*", 0 }, { "+", 1 }, { "(", 2 }, { ")", 3 }, { "id", 4 }, { "$", 5 } };

            _rowsDictionary =
                new Dictionary<string, int> { { "E", 0 }, { "E'", 1 }, { "T", 2 }, { "T'", 3 }, { "F", 4 } };
        }

        private static string[,] GetParsingTable()
        {
            return new[,]
            {
                {"error", "errror", "E->TE'", "error", "E->TE'", "error"},
                {"error", "E'->+TE'", "error", "E'->null", "error", "E'->null"},
                {"error", "error", "T->FT'", "error", "T->FT'", "error"},
                {"T'->*FT'", "T'->null", "error", "T'->null", "error", "T'->null"},
                {"error", "error", "F->(E)", "error", "F->id", "error"}
            };
        }
    }
}