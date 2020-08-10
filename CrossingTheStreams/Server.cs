using System;
using System.Collections.ObjectModel;
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
        private static ObservableCollection<(string, FileStream, long)> _receiving;
        private static (int, int) _windowSize = (0, 0);
        public static void NesdServer()
        {
            _receiving = new ObservableCollection<(string, FileStream, long)>();
            var listener = new TcpListener(IPAddress.Any,8000);
            listener.Start();
            Console.WriteLine("Server started.");
            Console.Clear();
            var uiThread = new Thread(ConsoleUI);
            uiThread.Start();
            while (true)
            {
                //If a connection is pending, start a new thread which then accepts it and receives the file
                if (listener.Pending()) 
                {
                    var receive = new Thread(() => ReceiveFile(listener));
                    receive.Start();
                }
            }
        }

        private static void ConsoleUI()
        {
            var listChanged = false;
            while (true)
            {
                //Probe if the amount of transferring items has changed, refresh console if so
                _receiving.CollectionChanged += (sender, args) => listChanged = true;
                if(listChanged) Console.Clear();
                listChanged = false;
                
                //probe if the size of the console window has changed, refresh console if so
                if (_windowSize != (Console.WindowWidth, Console.WindowHeight))
                {
                    _windowSize = (Console.WindowWidth, Console.WindowHeight);
                    Console.Clear();
                }
                
                Console.CursorVisible = false;
                Console.SetCursorPosition(2, 1);
                Console.ForegroundColor = ConsoleColor.Green;
                
                //Bit of a blanket statement, but if we're NOT ready i'm sure there will be other fun stuff on the screen
                Console.Write("Ready, accepting connections.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
                
                //Display receiving files from collection, with progress for each
                foreach (var item in _receiving)
                {
                    Console.Write("  ");
                    ProgressBar.InlineProgress((double) item.Item2.Position / item.Item3 * 100, Console.WindowWidth / 3, true);
                    Console.Write("receiving " + item.Item1);
                    Console.WriteLine();
                }

                Thread.Sleep(1000);
            }
        }

        private static void ReceiveFile(TcpListener listener)
        {
            FileStream file;
            var path = "";
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
                    //Read in the file attributes
                    var netstream = client.GetStream();
                    var nameLen = BitConverter.ToInt32(netstream.ReadExactly(4));
                    var name = Encoding.UTF8.GetString(netstream.ReadExactly(nameLen));
                    var len = BitConverter.ToInt64(netstream.ReadExactly(8));
                    
                    //generate destination path based on current user's desktop TODO:make this configurable to any location
                    path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), name);

                    //Sanity-check path, add file to list of transfers and copy it.
                    if (!string.IsNullOrEmpty(path))
                    {
                        file = File.OpenWrite(path);
                        var listitem = (name, file, len);
                        _receiving.Add(listitem);
                        var task = netstream.CopyToAsync(file);
                        task.Wait();
                        _receiving.Remove(listitem);


                        //Console.CursorVisible = false;
                        // do
                        // {
                        // ProgressBar.ShowProgress(Math.Ceiling((double) file.Position / len * 100), $"Receiving {name}...", true);
                        // Thread.Sleep(500);
                        // } while (!task.IsCompleted);
                        
                        if (task.IsCompletedSuccessfully)
                        {
                            Console.Clear();
                            file.Dispose();
                            Console.SetCursorPosition(2, Console.WindowHeight);
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

                        Console.SetCursorPosition(2, 1);
                        while (Console.CursorLeft <= Console.WindowWidth - 2)
                        {
                            Console.Write(" ");
                        }

                        break;
                    }

                    Console.SetCursorPosition(0, 0);
                    Console.WriteLine("Type something,dummy");
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