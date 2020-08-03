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

            FillBoxes();
            ShowStash();
            Console.WriteLine("\nYour boxes are full. We should make some room don't you think? Y/N\n"); //Empty the boxes in so desired. Rudely, that is.
            var key =Console.ReadKey();
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
            var container = new Container(1);
            Console.WriteLine("\nOh neat, a container showed up. Guess we should put some boxes in there. Wanna do it? Y/N\n");
            key = Console.ReadKey();
            if (key.KeyChar.ToString().ToLower() == "y")
            {
                LoadBoxes(container);
                Console.WriteLine("Container's loaded! Here's what's inside:\n");
                foreach (var box in container.Contents)
                {
                    Console.WriteLine($"{box.Colour} box with {box.Item}.");
                }
            }
            ShowStash();
            Console.WriteLine("\nNow what? World's your oyster!\n");
            while (true)
            {
                Console.WriteLine("1.Fill some new boxes.\n2.View your stash.\n3.Empty your boxes.\n4.Fill all your boxes with one thing.\n5.Put your boxes into your container.\n6.Take some boxes out of the container.");
                key = Console.ReadKey();
                switch (key.KeyChar.ToString().ToLower())
                {
                    case "1":
                        FillBoxes();
                        break;
                    case "2":
                        ShowStash();
                        break;
                    case "3":
                        EmptyBoxes();
                        break;
                    case "4":
                        Console.WriteLine("What to fill 'em with?");
                        var NewThing = Console.ReadLine();
                        FillAllBoxes(NewThing);
                        Console.WriteLine("it is done. Check your stash out!");
                        break;
                    case "5":
                        LoadBoxes(container);
                        break;
                    case "6":
                        Console.WriteLine("What box colour would you like to go through?");
                        var BoxColour = Console.ReadLine();
                        container.Unload(BoxColour);
                        break;
                    default:
                        break;
                }


            }
        }

        private static void LoadBoxes(Container container)
        {
            foreach (var box in Stash)
            {
                container.Load(box);
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
            Console.WriteLine("Your stash contains the following:\n"); //Show off your stash of boxes and their contents. You're so organized.
            foreach (var box in Stash)
            {
                box.Describe();
            }
        }

        private static void FillBoxes()
        {
            while (true)
            { //Make boxes in any colour you like, and put something in each one.
                Console.WriteLine("What's the colour of your box?");
                var colour = Console.ReadLine();

                Console.WriteLine("What's inside?");
                var contents = Console.ReadLine();

                var box = new Box(contents, colour);
                Stash.Add(box);

                Console.WriteLine($"\nCool, now you have a {box.Colour} box containing {box.Item}. Do you want another box to put something in? Y/N");
                var another = Console.ReadKey(true);
                if (another.KeyChar.ToString().ToLower() == "n") { break; }
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
    class Container
    {
        public List<Box> Contents { get; set; }
        public string ContainerNumber { get; set; }

        public Container(int CNumber)
        {
            Contents = new List<Box>();
            int ContainerNumber = CNumber;
        }

        public void Load(Box box) //todo add intermediary list so this does not throw an exception.
        {
            Contents.Add(box);
            Program.Stash.Remove(box);
        }

        public void Unload(string BoxColour)
        {
            List<Box> BoxesToUnload = Contents.FindAll(x => x.Colour == BoxColour);
            foreach (var box in BoxesToUnload)
            {
                Console.WriteLine($"There's a {box.Colour} box here that contains {box.Item}. Do you want to unload this? Y/N");
                var key = Console.ReadKey();
                if (key.KeyChar.ToString().ToLower() == "y")
                {
                    Contents.Remove(box);
                    Console.WriteLine("OK, got it out!");
                    Program.Stash.Add(box);
                }

            }
        }
    }
}
