using System;
using System.Text;

namespace ConTools
{
    public static class ProgressBar
    {
        private static (int, int) _windowSize = (0, 0);

        public static void ShowProgress(double percent, string message)
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
                Console.SetCursorPosition(Console.WindowWidth / 2 - 2, Console.WindowHeight - 1);
                Console.Write($"{percent}%");
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
    }
}