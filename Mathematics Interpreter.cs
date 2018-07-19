using System;

namespace BonusAssignmnet
{
    class Program
    {
        public static string Int = "Int",
            Plus = "Plus",
            End = "End",
            Minus = "Minus",
            Multiply = "Multiply",
            Divide = "Divide";

        static void Main(string[] args)
        {
            Interpreter interpreter;
            while (true)
            {
                Console.Write("Input: ");
                interpreter = new Interpreter(Console.ReadLine());
                Console.WriteLine(interpreter.Evaluate());
            }
        }
    }

    class Token
    {
        public string Type { get; set; }
        public char Value { get; set; }

        public Token(string type, char value)
        {
            Type = type;
            Value = value;
        }
    }

    class Interpreter
    {
        private readonly string _text;
        private int _position;
        private Token _currentToken;

        public Interpreter(string text)
        {
            _text = text;
        }

        private Token GetNextToken()
        {
            var text = _text;

            if (_position > text.Length - 1)
                return new Token(Program.End, '\0');

            var currentChar = text[_position];

            if (char.IsDigit(currentChar))
            {
                _position++;
                return new Token(Program.Int, currentChar);
            }

            if (currentChar == '+')
            {
                _position++;
                return new Token(Program.Plus, currentChar);
            }

            if (currentChar == '-')
            {
                _position++;
                return new Token(Program.Minus, currentChar);
            }
            if (currentChar == '*')
            {
                _position++;
                return new Token(Program.Multiply, currentChar);
            }

            if (currentChar == '/')
            {
                _position++;
                return new Token(Program.Divide, currentChar);
            }

            ThrowParException();
            return null;
        }

        private void MatchType(string tokenType)
        {
            if (_currentToken.Type == tokenType)
                _currentToken = GetNextToken();
            else
                ThrowParException();
        }

        private void ThrowParException()
        {
            throw new ParseException("Error parsing input");
        }

        public int Evaluate()
        {
            string left = string.Empty, right = string.Empty;

            _currentToken = GetNextToken();

            while (_currentToken.Type.Equals(Program.Int))
            {
                left += _currentToken.Value;
                MatchType(Program.Int);
            }

            var op = _currentToken;

            if (op.Type.Equals(Program.End))
                ThrowParException();

            else if (op.Type.Equals(Program.Plus))
                MatchType(Program.Plus);

            else if (op.Type.Equals(Program.Minus))
                MatchType(Program.Minus);

            else if (op.Type.Equals(Program.Multiply))
                MatchType(Program.Multiply);

            else if (op.Type.Equals(Program.Divide))
                MatchType(Program.Divide);

            while (!_currentToken.Type.Equals(Program.End))
            {
                right += _currentToken.Value;
                MatchType(Program.Int);
            }

            if (op.Type.Equals(Program.Plus))
                return GetInt(left) + GetInt(right);

            if (op.Type.Equals(Program.Minus))
                return GetInt(left) - GetInt(right);

            if (op.Type.Equals(Program.Multiply))
                return GetInt(left) * GetInt(right);

            if (op.Type.Equals(Program.Divide))
                return GetInt(left) / GetInt(right);

            ThrowParException();

            return -1;
        }

        private int GetInt(string num)
        {
            int temp = 0;
            foreach (var n in num)
                temp = (temp * 10) + (n - 48);
            return temp;
        }
    }

    class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {

        }
    }
}