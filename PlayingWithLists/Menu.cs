using System;
using System.Collections.Generic;
using System.Linq;

namespace PlayingWithLists
{
    public class Menu
    {
        public static void ShowMenu(List<(string,Action)> options)
        {

            var optNo = 0;
            foreach (var option in options)
            {
                Console.WriteLine($"{++optNo}. {option.Item1}");
            }

            Console.WriteLine($"\nPlease select an option [ 1 - {options.Count} ]\n");
            var choice = 0;
            while (true)
            {
                var input = Console.ReadLine() ?? "0";
                if (input != "" && input.ToCharArray().All(char.IsNumber))
                {
                    choice = int.Parse(input);
                    if (choice > 0 && choice <= options.Count)
                    {
                        break;
                    }
                }

                Console.WriteLine($"That didn't work. Please input a number from 1 to {options.Count}.");
            }

            options[choice - 1].Item2.Invoke();
        }
    }
}