using System;
using System.Collections.Generic;

namespace BonusAssignmnet
{
    class Program
    {
        private static readonly Stack<int> Stack = new Stack<int>();
        private static readonly Queue<char> Input = new Queue<char>();
        private static readonly Stack<char> Symbol = new Stack<char>();
        private static string[,] _parsingTable;
        private static Dictionary<char, int> _columnsDictionary;
        private static int _row = 0, _col = 0;

        static void Main(string[] args)
        {
            InitializeColumnsDictionary();

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
            while (Symbol.Count != 0) Symbol.Pop();
        }

        private static void ParseInput(string raw)
        {
            foreach (var t in raw)
                Input.Enqueue(t);

            Input.Enqueue('$');

            _parsingTable = GetParsingTable();

            Stack.Push(0);
            while (true)
            {
                _row = Stack.Peek();
                _col = _columnsDictionary[Input.Peek()];

                var data = _parsingTable[_row, _col];
                if (data.Contains("S") && data.Length == 2) Shift(data);
                else if (data.Contains("->")) Reduce(data);
                else if (data == "accept")
                {
                    Console.WriteLine("Parsed successfully.");
                    break;
                }
                else
                {
                    Console.WriteLine("Error");
                    break;
                }
            }
        }

        private static void InitializeColumnsDictionary()
        {
            _columnsDictionary =
                new Dictionary<char, int> {{'a', 0}, {'b', 1}, {'$', 2}, {'S', 3}};
        }

        private static void Shift(string input)
        {
            var top = Input.Peek();
            Symbol.Push(top);
            Stack.Push(input[1] - 48);
            Input.Dequeue();
        }

        private static void Reduce(string input)
        {
            var head = input.Split('>')[0][0];
            var body = input.Split('>')[1];

            var t = body.Length;
            for (var i = 0; i < t; i++)
                Symbol.Pop();

            Symbol.Push(head);

            for (var i = 0; i < t; i++)
                Stack.Pop();

            _row = Stack.Peek();
            _col = _columnsDictionary[Symbol.Peek()];

            Stack.Push(_parsingTable[_row, _col][1] - 48);
        }

        private static string[,] GetParsingTable()
        {
            return new[,]
            {
                {"S2", "S3", "error", "S1"},
                {"error", "error", "accept", "error"},
                {"S2", "S3", "error", "S4"},
                {"S->b", "S->b", "S->b", "S->b"},
                {"S->as", "S->as", "S->as", "S->as"}
            };
        }
    }
}
