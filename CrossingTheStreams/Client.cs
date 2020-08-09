using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CrossingTheStreams
{
    class Client
    {
        public static void NesdClient()
        {
            FileStream file;
            long filesize;
            Console.WriteLine("What file do you want to send?");
            while (true)
            {
                var path = Console.ReadLine();

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
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
                if (!string.IsNullOrEmpty(address) && IPAddress.TryParse(address, out ipaddress))
                {

                    try
                    {
                        var client = new TcpClient();
                        Console.WriteLine("Connecting...");
                        client.Connect(ipaddress, 8000);
                        var netstream = client.GetStream();
                        Task task = file.CopyToAsync(netstream);
                        Console.Clear();
                        Console.CursorVisible = false;
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
                        Console.CursorVisible = true;
                        client.Close();
                        Console.Clear();
                        if (task.IsCompletedSuccessfully)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("File sent OK!\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                            break;
                        }
                        else
                        {
                            client.Dispose();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(
                                "Something went wrong. Check the network and receiver, then try again.\n");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                    catch (IOException)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong. Check the file, network and receiver, then try again.\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                    catch (SocketException)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("A network error occured. Check the IP address and connection, then try again.\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    }
                }
                else
                {
                    Console.WriteLine("Learn to type, dummy");
                }
            }

            Program.Main();
        }
    }
}