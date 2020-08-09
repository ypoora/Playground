using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ConTools;

namespace CrossingTheStreams
{
    class Server
    {
        public static void NesdServer()
        {
            FileStream file;
            var listener = new TcpListener(IPAddress.Any,8000);
            listener.Start();
            Console.WriteLine("Server started.");
            Console.Clear();
            while (true)
            {
                Console.CursorVisible = false;
                Console.SetCursorPosition(2,1);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write ("Ready, awaiting connection.");
                Console.ForegroundColor = ConsoleColor.Gray;
                Thread.Sleep(250);
                var path = "";
                if (listener.Pending())
                {
                    try
                    {
                        var client = listener.AcceptTcpClient();
                        //Console.SetCursorPosition(2,1);
                        //Console.ForegroundColor = ConsoleColor.Yellow;
                        //Console.Write("Client connecting... Enter path to save file: ");
                        //Console.CursorVisible = true;
                        //Console.ForegroundColor = ConsoleColor.Gray;
                        while (true)
                        {
                            //TODO: turn these into extension methods for code cleanness
                            //Read in the 
                            var netstream = client.GetStream();
                            var nameLenBuf = new byte[4];
                            netstream.Read(nameLenBuf, 0, 4);
                            var nameLen = BitConverter.ToInt32(nameLenBuf);
                            var nameBuf = new byte[nameLen];
                            netstream.Read(nameBuf, 0, nameLen);
                            var name = Encoding.UTF8.GetString(nameBuf);
                            var lenBuf = new byte[8];
                            netstream.Read(lenBuf, 0, 8);
                            var len = BitConverter.ToInt64(lenBuf);


                            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), name);
                            //path = Console.ReadLine();
                            if (!string.IsNullOrEmpty(path))
                            {
                                //Console.CursorVisible = false;
                                Console.SetCursorPosition(2,2);
                                Console.WriteLine("Receiving file...");
                                file = File.OpenWrite(path + name);
                                var task = netstream.CopyToAsync(file);
                                Console.Clear();
                                //Console.CursorVisible = false;
                                do
                                {
                                    ProgressBar.ShowProgress(Math.Ceiling((double) file.Position / len * 100), $"Receiving {name}...", true);
                                    Thread.Sleep(500);
                                } while (!task.IsCompleted);

                                if (task.IsCompletedSuccessfully)
                                {
                                    Console.Clear();
                                    file.Dispose();
                                    Console.SetCursorPosition(2, 2);
                                    Console.WriteLine($"File {name} was stored.");
                                }
                                else
                                {
                                    file.Dispose();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine("Something went wrong. Check the network and sender, then try again.\n");
                                    Console.ForegroundColor = ConsoleColor.Gray;
                                    Console.WriteLine("Deleting the failed partial file... ");
                                    try
                                    {
                                        File.Delete(path);
                                        Console.WriteLine("File deleted.");
                                    }
                                    catch
                                    {
                                        Console.WriteLine("Deletion failed. File may not have been created yet.");
                                    }

                                    Thread.Sleep(1000);
                                    Console.Clear();
                                }

                                Console.SetCursorPosition(2,1);
                                while (Console.CursorLeft <= Console.WindowWidth - 2)
                                {
                                    Console.Write(" ");
                                }
                                break;
                            }
                            else
                            {
                                Console.SetCursorPosition(0,0);
                                Console.WriteLine("Type something,dummy");
                            }
                        }
                    }
                    catch (IOException)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Something went wrong. Check the network and sender, then try again.\n");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Deleting the failed partial file... ");
                        try
                        {
                            File.Delete(path);
                            Console.WriteLine("File deleted.");
                        }
                        catch
                        {
                            Console.WriteLine("Deletion failed. File may not have been created yet.");
                        }

                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
            }
        }
    }
}