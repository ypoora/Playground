using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ConTools;

namespace CrossingTheStreams
{
    class Program
    {
        static void Main()
        {
            var opts = new List<(string, Action)>
            {
                ("Client", NesdClient),
                ("Server", NesdServer),
                ("Quit", () => Environment.Exit(0))
            };
            Console.WriteLine("What should i do?\n");
            Menu.ShowMenu(opts);
        }

        private static void NesdClient()
        {
            FileStream file;
            long filesize;
            Console.WriteLine("What file do you want to send?");
            while (true)
            {
                var path = Console.ReadLine();

                if (File.Exists(path))
                {
                    file = File.OpenRead(path);
                    filesize = new FileInfo(path).Length;
                    break;
                }
                else
                {
                    Console.WriteLine("Learn to type, dummy");
                }
            }

            Console.WriteLine("Where's it gotta go? Please specify an IP address.");
            while (true)
            {
                var address = Console.ReadLine();
                IPAddress ipaddress;
                if (IPAddress.TryParse(address, out ipaddress))
                {

                    try
                    {
                        var client = new TcpClient();
                        Console.WriteLine("Connecting...");
                        client.Connect(ipaddress, 8000);
                        var netstream = client.GetStream();
                        Task task = file.CopyToAsync(netstream);
                        Console.Clear();
                        Console.WriteLine("Transferring... ");
                        var origwidth = Console.WindowWidth;
                        do
                        {
                            Thread.Sleep(500);
                            if (Console.WindowWidth < origwidth)
                            {
                                Console.SetWindowSize(origwidth, Console.WindowHeight);
                            }

                            var percent = Math.Round(file.Position / (double) filesize * 100);
                            Console.SetCursorPosition(16, 0);
                            Console.Write((percent + "%").PadLeft(4));
                            Console.SetCursorPosition(0, Console.WindowHeight - 1);
                            while (Console.CursorLeft <= Console.WindowWidth - 2)
                            {
                                double barposition = Console.WindowWidth * percent / 100;
                                if (Console.CursorLeft <= barposition)
                                {
                                    Console.Write("=");
                                }
                                else
                                {
                                    Console.Write(" ");
                                }
                            }
                        } while (!task.IsCompleted);
                        client.Close();
                        Console.Clear();
                        if (task.IsCompletedSuccessfully)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("File sent OK!\n");
                            Console.ForegroundColor = ConsoleColor.Gray
                            break;
                        }
                        else
                        {
                            client.Dispose();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Something went wrong. Check the network and receiver, then try again.\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                    catch (IOException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong. Check the network and receiver, then try again.\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                else
                {
                    Console.WriteLine("Learn to type, dummy");
                }
            }
            
            Main();
        }


        private static void NesdServer()
        {
            FileStream file;
            var listener = new TcpListener(8000);
            listener.Start();
            Console.WriteLine("Server started.");
            while (true)
            {
                Console.SetCursorPosition(2,Console.WindowHeight - 2);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write ("Ready, awaiting connection.");
                Console.ForegroundColor = ConsoleColor.Gray;
                var path = "";
                if (listener.Pending())
                {
                    try
                    {
                        var client = listener.AcceptTcpClient();
                        Console.SetCursorPosition(2,Console.WindowHeight - 2);
                        Console.ForegroundColor = ConsoleColor.Yellow
                        Console.Write("Client connecting... Enter path to save file: ");
                        Console.ForegroundColor = ConsoleColor.Gray
                        while (true)
                        {
                            path = Console.ReadLine();
                            if (!string.IsNullOrEmpty(path))
                            {
                                Console.WriteLine("Receiving file...");
                                file = File.OpenWrite(path);
                                var netstream = client.GetStream();
                                netstream.CopyTo(file);
                                file.Dispose();
                                Console.WriteLine($"File stored.");
                                Thread.Sleep(2000);
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Type something,dummy");
                            }
                        }
                    }
                    catch (IOException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong. Check the network and sender, then try again.\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Deleting the failed partial file.");
                        try
                        {
                            File.Delete(path);
                        }
                        catch
                        {
                        }

                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }
        }
    }
}