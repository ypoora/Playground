using System;
using System.Linq;
using System.Text;

namespace ConTools
{
    public static class ProgressBar
    {
        private static (int, int) _windowSize = (0, 0);
        /// <summary>
        /// Shows a full-screen progress bar to the user with a percentage display and a customizable message.
        /// </summary>
        /// <param name="percent">Percentage to draw (0-100, accepts doubles)</param>
        /// <param name="message">Message to show to user whilst your process runs</param>
        /// <param name="showPercent">Display percentage true/false</param>
        public static void ShowProgress(double percent, string message, bool showPercent)
        {
            
            if (Console.WindowWidth < 22 && Console.WindowHeight < 5) return;
            if (_windowSize != (Console.WindowWidth, Console.WindowHeight))
            {
                _windowSize = (Console.WindowWidth, Console.WindowHeight);
                Console.Clear();
                Console.SetCursorPosition( (Console.WindowWidth / 2) - (message.Length / 2), Console.WindowHeight - 3);
                Console.Write($"{message}");
            }

            try
            {
                if(showPercent) Console.SetCursorPosition(Console.WindowWidth / 2 - 2, Console.WindowHeight - 1);
                if(showPercent) Console.Write($"{Math.Floor(percent)}%");
                Console.SetCursorPosition(0, Console.WindowHeight - 2);
                var barLength = Math.Floor(Console.WindowWidth * (percent / 100));
                var builder = new StringBuilder();
                for (var i = 0; i < barLength; i++)
                {
                    builder.Append("=");
                }

                Console.Write(builder.ToString().PadRight(Console.WindowWidth));
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Clear();
            }
        }

        /// <summary>
        /// Prints an in-line progress bar with configurable length and percentage.
        /// </summary>
        /// <param name="percent">Percentage to draw (0-100, accepts doubles)</param>
        /// <param name="length">Length of the bar.</param>
        /// <param name="showPercent">Display percentage true/false</param>
        public static void InlineProgress(double percent, int length, bool showPercent)
        {
            var barLength = Math.Floor(length * (percent / 100));
            var builder = new StringBuilder();
            for (var i = 0; i < barLength; i++)
            {
                builder.Append("=");
            }
            Console.Write("[" + builder.ToString().PadRight(length) + "]");
            if (showPercent) Console.Write($" {(Math.Floor(percent)+ "%").PadRight(4)}");
        }
    }
}