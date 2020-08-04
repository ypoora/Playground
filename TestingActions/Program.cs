using System;

namespace TestingActions
{
    class Program
    {
        static void Main(string[] args)
        {
            Action<int, int> action = (x, y) => Console.WriteLine($"i was invoked, and my number is {x}. I found a {y} on the floor just now. If i were to add that to my {x} i'd have {x + y}!");
            Console.WriteLine($"Hello World,");
            action.Invoke(5, 9);
        }
    }
}
