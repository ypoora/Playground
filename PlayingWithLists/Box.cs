using System;

namespace PlayingWithLists
{
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
