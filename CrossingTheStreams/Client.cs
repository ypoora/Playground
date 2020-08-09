using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConTools;

namespace CrossingTheStreams
{
    class Client
    {
        public static void NesdClient()
        {
            FileStream file;
            long filesize;
            string filename;
            Console.WriteLine("What file do you want to send?");
            while (true)
            {
                var path = Console.ReadLine();

                if (!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    //read file, get details to give to Transmit method
                    file = File.OpenRead(path);
                    filesize = new FileInfo(path).Length;
                    filename = new FileInfo(path).Name;
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
                    //If IP is valid, off we go
                    Transmit(ipaddress, file, filename, filesize);
                    break;
                }
                Console.WriteLine("Learn to type, dummy");
            }

            Program.Main();
        }

        private static void Transmit(IPAddress ipaddress, FileStream file,string name, long filesize)
        {
            try
            {
                //Connect to server
                var client = new TcpClient();
                Console.WriteLine("Connecting...");
                client.Connect(ipaddress, 8000);
                var netstream = client.GetStream();
                Console.Write(" OK.");
                
                //Transmit file details
                byte[] namelengthbyte = BitConverter.GetBytes(name.Length);
                byte[] namebyte = Encoding.UTF8.GetBytes(name);
                byte[] filelengthbyte = BitConverter.GetBytes(file.Length);
                netstream.Write(namelengthbyte.Concat(namebyte.Concat(filelengthbyte)).ToArray());
                
                //transmit data blob with progressbar
                Task task = file.CopyToAsync(netstream);
                Console.Clear();
                Console.CursorVisible = false;
                do
                {
                    ProgressBar.ShowProgress(Math.Ceiling((double) file.Position / filesize * 100), $"Transferring {name}...", true);
                    Thread.Sleep(500);
                } while (!task.IsCompleted);

                Console.CursorVisible = true;
                
                //Clean up and tell the user we're done here
                client.Close();
                Console.Clear();
                if (task.IsCompletedSuccessfully)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("File sent OK!\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    return;
                }

                //In case of error, clean up and tell the user we've had a problem
                client.Dispose();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(
                    "Something went wrong. Check the network and receiver, then try again.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (IOException) //Rare, but i've seen it happen so this is here now
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong. Check the file, network and receiver, then try again.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (SocketException) //Error out on connection issues and tell the user we've had a problem
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("A network error occured. Check the IP address and connection, then try again.\n");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }
    }
}