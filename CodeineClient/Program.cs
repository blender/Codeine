using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Codeine;
using System.IO;

namespace CodeineClient
{
    class Program
    {

        private const int udpPortSrv = 4568;
        private const int udpPortCli = 4569;
        private static bool dataReceived;
        private static bool loop;
        
        static void Main(string[] args)
        {
            loop = true;
            do{
                dataReceived = false;
                Console.WriteLine("Connectig to localhost...");
                UdpClient uClient;
                try
                {
                    uClient = new UdpClient();
                    uClient.Connect("localhost", udpPortSrv);
                }
                catch (System.Net.Sockets.SocketException e) {

                    Console.WriteLine("Connection Failed: {0}", e.ToString());
                    return;
                }

                Console.WriteLine("Connection succeded!");
                Console.WriteLine("Press Enter to start handshake...");
                Console.ReadLine();
                Console.WriteLine("Handshaking...");
                string handShakeSting = "hello!";
                uClient.Send(Encoding.ASCII.GetBytes(handShakeSting), handShakeSting.Length);
                Console.WriteLine("Handshake completed!");
                IPEndPoint remote = new IPEndPoint(IPAddress.Any, udpPortCli);
                Console.WriteLine("Waiting for data...");
                
                byte[] data;

                try
                {
                    data = uClient.Receive(ref remote);
                    PackedContactDescriptors packedDescs = StructConverter.fromArray(data);
                    uClient.Close();
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Socket Raised exception in receive {0}", e.ToString());
                }

                Console.WriteLine("Loop? Y/n");
                string ans = Console.ReadLine();

                if (ans.Equals("n")) 
                    loop = false;

            } while(loop);
 
        }

 
    }
}
