using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;

namespace PlayingWithLists
{
    class Program
    {
        public static List<Box> Stash { get; set; }

        static void Main(string[] args)
        {
            Stash = new List<Box>(); //Make a list. Lists are good places to stash boxes. I swear.
            var container = new Container(); //Make a container, you know,for containing.
            Tutorial(container);

            while (true) //Main menu loop.
            {
                Console.WriteLine("\n1.Fill some new boxes.\n2.View your stash.\n3.Empty your boxes.\n4.Fill all your boxes with one thing.\n5.Put your boxes into your container.\n6.Take some boxes out of the container.\n7. Show what's in the container.\n");
                var key = Console.ReadKey(true);
                Console.Clear();
                switch (key.KeyChar.ToString().ToLower())
                {
                    case "1":
                        FillBox();
                        break;
                    case "2":
                        ShowStash();
                        break;
                    case "3":
                        EmptyBoxes();
                        Console.WriteLine("Took everything out. Threw it overboard. These items will never be seen again!");
                        ShowStash();
                        break;
                    case "4":
                        Console.WriteLine("What to fill 'em with?\n");
                        var NewThing = Console.ReadLine();
                        FillAllBoxes(NewThing);
                        Console.WriteLine("it is done. Check your stash out!\n");
                        break;
                    case "5":
                        LoadBoxes(container);
                        break;
                    case "6":
                        Console.WriteLine("What box colour would you like to go through?");
                        var BoxColour = Console.ReadLine();
                        container.Unload(BoxColour);
                        break;
                    case "7":
                        container.ListContents();
                        break;
                    default:
                        break;
                }
            }
        }

        private static void Tutorial(Container container)
        {
            Console.WriteLine("Imagine this. You have boxes available in any colour you like.");
            Console.WriteLine("You have a bunch of items you could stash in there!");
            Console.WriteLine("Let's put ten items into ten coloured boxes. Go!");
            int i = 0;
            while (i < 10)
            {
                FillBox();
                i++;
            }
            ShowStash();
            Console.WriteLine("\nYour boxes are full. We should make some room don't you think? Y/N\n"); //Empty the boxes if so desired. Rudely, that is.
            var key = Console.ReadKey(true);
            if (key.KeyChar.ToString().ToLower() == "y")
            {
                Console.WriteLine("These items were not important, were they?\n"); //oops ;)
                EmptyBoxes();
                ShowStash();
                Console.WriteLine("Now tell me something you really really like a lot.");
                var NewThing = Console.ReadLine();
                Console.WriteLine("\nCool. Did ya a favour; here's your new stash!\n"); //I'm going to put your favourite thing into all of your boxes. Aren't i nice?
                FillAllBoxes(NewThing);
                ShowStash();
            }
            Console.WriteLine("\nYou also have a storage container. Guess we should put some boxes in there. Wanna do it? Y/N\n");
            key = Console.ReadKey(true);
            if (key.KeyChar.ToString().ToLower() == "y")
            {
                LoadBoxes(container);
                Console.WriteLine("\nContainer's loaded! Here's what's inside:\n");
                foreach (var box in container.Contents)
                {
                    Console.WriteLine($"{box.Colour} box with {box.Item}.");
                }
            }
            ShowStash();
            Console.WriteLine("\nNow what? World's your oyster!\n");
        }

        private static void LoadBoxes(Container container)
        {
            foreach (var box in Stash)
            {
                container.Load(box);
                Console.WriteLine($"The {box.Colour} box with {box.Item} was loaded.");
            }
            foreach (var box in container.ToRemoveFromStash)
            {
                Stash.Remove(box);
            }

        }

        private static void FillAllBoxes(string NewThing)
        {
            foreach (var box in Stash)
            {
                box.Fill(NewThing);
            }
        }

        private static void EmptyBoxes()
        {
            foreach (var box in Stash)
            {
                box.Empty();
            }
        }

        private static void ShowStash()
        {
            Console.WriteLine("\nYour stash contains the following:\n"); //Show off your stash of boxes and their contents. You're so organized.
            foreach (var box in Stash)
            {
                box.Describe();
            }
            Console.WriteLine();
        }

        private static void FillBox()
        {
                Console.WriteLine("\nWhat's the colour of your box?");
                var colour = Console.ReadLine();

                Console.WriteLine("\nWhat's inside?");
                var contents = Console.ReadLine();

                var box = new Box(contents, colour);
                Stash.Add(box);

                Console.WriteLine($"\nCool, now you have a {box.Colour} box containing {box.Item}.\n");
        }
    }
}
