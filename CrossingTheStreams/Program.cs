using System;
using System.Collections.Generic;
using ConTools;

namespace CrossingTheStreams
{
    class Program
    {
        public static void Main()
        {
            var opts = new List<(string, Action)>
            {
                ("Client", Client.NesdClient),
                ("Server", Server.NesdServer),
                ("Quit", () => Environment.Exit(0))
            };
            Console.WriteLine("What should i do?\n");
            Menu.ShowMenu(opts);
        }
    }
}