using System;

namespace BonusAssignmnet
{
    class Program
    {
        static void Main(string[] args)
        {
            Exponential exponentialObj = new Exponential();

            while (true)
            {
                Console.Write("Input: ");
                exponentialObj.ConvertScientificNotationToExpression(Console.ReadLine());
                Console.WriteLine();
            }
        }

        class Exponential
        {
            public void ConvertScientificNotationToExpression(string input)
            {
                string[] opts = input?.Split('E', '.');

                if (opts?.Length == 3)// with .
                {
                    long exponential = ConvertToInt(opts[2]);
                    if (exponential >= 0) //for positive exponential
                    {
                        int lengthOfExponent = opts[1].Length;
                        int countsToMoveFloatingPoint = (int)exponential - lengthOfExponent;
                        if (countsToMoveFloatingPoint > 0)
                        {
                            for (int i = 0; i < countsToMoveFloatingPoint; i++)
                                opts[1] = opts[1] + "0";

                            string number = opts[0] + opts[1];
                            Console.WriteLine("Output: " + ConvertToInt(number));
                        }
                        else
                        {
                            string number = string.Empty;
                            for (int i = 0; i < opts[1].Length; i++)
                            {
                                if (i == exponential)
                                    number = number + ".";

                                number = number + opts[1][i];
                            }

                            Console.WriteLine("Output: " + opts[0] + number);
                        }
                    }
                    else //for negative exponential
                    {
                        long inputLenght = opts[0].Length;
                        long result = inputLenght + exponential;
                        if (result > 0)
                        {
                            opts[0] = opts[0].Insert((int)result, ".");
                            Console.WriteLine("Output: " + opts[0] + opts[1]);
                        }
                        else if (result < 0)
                        {
                            result *= -1;
                            for (int i = 0; i < result; i++)
                                opts[0] = "0" + opts[0];
                            opts[0] = "0." + opts[0];
                            Console.WriteLine("Output: " + opts[0] + opts[1]);
                        }
                    }
                }
                else if (opts?.Length == 2) //without .
                {
                    long exponential = ConvertToInt(opts[1]);
                    if (exponential >= 0) //for positive exponential
                    {
                        for (int i = 0; i < exponential; i++)
                            opts[0] += "0";
                        long number = ConvertToInt(opts[0]);
                        Console.WriteLine("Output: " + number);
                    }
                    else //for negative exponential
                    {
                        long inputLenght = opts[0].Length;
                        long result = inputLenght + exponential;
                        if (result > 0)
                        {
                            opts[0] = opts[0].Insert((int)result, ".");
                            Console.WriteLine("Output: " + opts[0]);
                        }
                        else if (result < 0)
                        {
                            result *= -1;
                            for (int i = 0; i < result; i++)
                                opts[0] = "0" + opts[0];
                            opts[0] = "0." + opts[0];
                            Console.WriteLine("Output: " + opts[0]);
                        }
                    }
                }
                else //invalid input
                    Console.WriteLine("Invalid input");
            }
            private long ConvertToInt(string number)
            {
                bool hasNegative = false;
                if (number[0] == '-')
                {
                    number = number.Remove(0, 1);
                    hasNegative = true;
                }

                long val = 0;
                foreach (var item in number)
                    val = val * 10 + (item - 48);

                if (hasNegative)
                    val *= -1;

                return val;
            }
        }
    }
}