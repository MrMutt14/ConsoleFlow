using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
namespace ConsoleFlow
{
    class Program
    {
        
        private static readonly Socket ClientSocket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private const int PORT = 9342;

        static void Main()
        {
            Console.Title = "ConsoleFlow";
            Console.WriteLine("--Welcome to ConsoleFlow--");
            Console.WriteLine("a free opensource Console RAT developed by FORC3FI3LD");
            Console.WriteLine("github.com/MrMutt14/ConsoleFlow");
            Console.WriteLine("Please donate to keep the develompent of this program! (donation link in github)");
            Console.WriteLine("");
            Console.Title = "ConsoleFlow - A Console RAT Developed by FORC3FI3LD";
            Thread.Sleep(500);
            ConnectToServer();
            RequestLoop();
            //Exit();
        }

        private static void ConnectToServer()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    ClientSocket.Connect(IPAddress.Loopback, PORT);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            }

            Console.Clear();
            Console.WriteLine("Connected");
        }

        private static void RequestLoop()
        {
            Console.WriteLine(@"<Type ""exit"" to properly disconnect client>");

            while (true)
            {
                SendRequest();
                ReceiveResponse();
            }
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private static void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }

        private static void SendRequest()
        {
            a:
            Console.WriteLine("Write /help for help!");
            Console.WriteLine("");
            
            Console.WriteLine("Write a command:");
            string request = Console.ReadLine();
            if (request == "/help")
            {
                Console.WriteLine("----HELP MENU----");
                Console.WriteLine("/ip - Gives the clients ip");
                Console.WriteLine("/time - Gives the UTC time of the client");
                Console.WriteLine("/os - Gives the OS the client");
                Console.WriteLine("/msg - Gives the OS the client");
                goto a;
            }
            
            else
            {
                #region actual sending
                SendString(request);

                if (request.ToLower() == "/exit")
                {
                    Exit();
                }
                #endregion
            }
        }

        /// <summary>
        /// Sends a string to the server with ASCII encoding.
        /// </summary>
        private static void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            
        }

        private static void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Console.WriteLine("");
            Console.WriteLine("Response from client:");
            Console.WriteLine(text);
        }
    }
}
    

