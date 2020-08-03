using System;
using System.IO;
using System.Linq;

namespace Idiosyncracies
{
    class Program
    {
        static void Main(string[] args)
        {
            //define score, set it to 0 and ask for participant name
            var score = 0;
            Console.WriteLine("Hi, what's your name?\n");
            var name = Console.ReadLine();
            Console.WriteLine($"\nHello {name}. \nNow we are going to do some interesting quizzery so you had better be ready.\n\n"); 
            var questions = File.ReadAllLines("questions.csv");  //Read in questions.csv
            foreach (var line in questions.Skip(1)) //Step through all questions in questions.csv skipping the first line as it is purely descriptive.
            {
                var question = line.Split(";"); //Grab a line from questions.csv and break the questions and answers into seperate pieces before writing them out
                Console.WriteLine($"Okay {name}, {question[0]}?\n");
                Console.WriteLine($"1. {question[1]}");
                Console.WriteLine($"2. {question[2]}");
                Console.WriteLine($"3. {question[3]}");
                Console.WriteLine($"4. {question[4]}\n");
                Console.WriteLine("Please press the number that corresponds to your answer.\n");
                var ans = Console.ReadKey(true);
                if (ans.KeyChar.ToString() == question[5]) //Comparing the quizzee's answer to whatever it was supposed to be acccording to questions.csv and giving them points or telling them they're trash
                {
                    score++;
                    Console.WriteLine($"Good job {name}, you got it right. Your score is now {score}.\n");
                }
                else
                {
                    Console.WriteLine($"{name}, {name}, {name}... Unbelievable. I can't believe you wouldn't know every bit of trivia out there!\n");
                }

            }
            Console.WriteLine($"Good job {name}, you made it through all of the questions with a final score of {score}."); //Write final score to quizzee before quitting
        }
    } 
}
