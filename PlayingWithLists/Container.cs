using System;
using System.Collections.Generic;

namespace PlayingWithLists
{
    class Container
    {
        public List<Box> Contents { get; set; }
        public List<Box> ToRemoveFromStash { get; set; }
        //public string ContainerNumber { get; set; } unneeded for now

        public Container()
        {
            Contents = new List<Box>();
            ToRemoveFromStash = new List<Box>();
            // int ContainerNumber = CNumber;
        }

        public void Load(Box box) //todo add intermediary list so this does not throw an exception.
        {
            Contents.Add(box);
            ToRemoveFromStash.Add(box);
        }

        public void Unload()
        {
            var BoxColour = Console.ReadLine();
            List<Box> BoxesToUnload = Contents.FindAll(x => x.Colour == BoxColour);
            foreach (var box in BoxesToUnload)
            {
                Console.WriteLine("What box colour would you like to go through?");
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
        public void ListContents()
        {
            Console.WriteLine("\nThe container contains:\n");
            foreach (var box in Contents)
            {
                Console.WriteLine($"A {box.Colour} box of {box.Item}.");
            }
        }
    }
}
