using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace PlayingWithLists
{
    class Program
    {
        static void Main(string[] args)
        {
            var Boxes = new List<Box>(); //Make a list. Lists are good places to stash boxes. I swear.

            while(true) { //Make boxes in any colour you like, and put something in each one.
                Console.WriteLine("What's the colour of your box?");
                var colour = Console.ReadLine();

                Console.WriteLine("What's inside?");
                var contents = Console.ReadLine();

                var Box = new Box(contents, colour);
                Boxes.Add(Box);

                Console.WriteLine($"\nCool, now you have a {Box.Colour} box containing {Box.Item}. Do you want another box to put something in? Y/N");
                var another = Console.ReadKey(true);
                if (another.KeyChar.ToString().ToLower() == "n") { break; }
            }
            Console.WriteLine("You have the following packed away in your stash:\n"); //Show off your stash of boxes and their contents. You're so organized.
            foreach(var Box in Boxes)
            {
                Box.Describe();
            }
            Console.WriteLine("\nYour boxes are full. We should make some room. These items were not important, were they?"); //Too bad i'm a dick, i don't care about your items at all!!!
            Console.WriteLine("Now we have:\n");
            foreach (var Box in Boxes)
            {
                Box.Empty();
                Box.Describe();
            }
            Console.WriteLine("Now tell me something you really really like a lot.");
            var NewThing = Console.ReadLine();
            Console.WriteLine("\nCool. Did ya a favour; here's your new stash!\n"); //I'm going to put your favourite thing into all of your boxes. Aren't i nice?
            foreach(var Box in Boxes)
            {
                Box.Fill(NewThing);
                Box.Describe();
            }
            

        }
    }
    class Box
    {
        public string Item { get; private set; }  //Contents of box
        public string Colour { get; private set; } //Colour of box

        public Box(string item, string colour)  //Constructor: Give box a colour and some contents
        {
            Item = item;
            Colour = colour;
        }

        public void Describe() //Tell me what is in my box!
        {
            Console.WriteLine($"A {Colour} box with {Item} inside.");
        }

        public void Empty() //Take everything out of my box!
        {
            Item = "nothing";
        }

        public void Fill(string item) //Put something into my box!
        {
            Item = item;
        }
    }
}
