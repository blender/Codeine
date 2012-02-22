using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.IO;


namespace Codeine
{
    public struct UdpState 
    {
        public UdpClient u { get; set; }
        public IPEndPoint e { get; set; }
    }


   
    class ServerController
    {
        private static ServerController instance;

        private const int udpPort = 4568;
        private UdpClient uClient;
        private ContacDataController cDataController;

        private ServerController() 
        {
            IPEndPoint e = new IPEndPoint(IPAddress.Any, udpPort);
            uClient = new UdpClient(udpPort);
            cDataController = ContacDataController.Instance;
            
            UdpState s = new UdpState();
            s.e = e;
            s.u = uClient;

            uClient.BeginReceive(new AsyncCallback(onRecevice), s);
        }

        public static ServerController Instance 
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServerController();
                }

                return instance;
            }
        }

        private void onRecevice(IAsyncResult res)
        {
            UdpClient u = (UdpClient)((UdpState)(res.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(res.AsyncState)).e;
            byte[] data = u.EndReceive(res, ref e);

            PackedContactDescriptors descs = cDataController.packedDescriptors;


            byte[] dataToSend = StructConverter.ToByteArray(descs);
           

            UdpState s = new UdpState();
            s.e = e;
            s.u = u;

            u.BeginSend(dataToSend, dataToSend.Length, e, onSend, s);

            s.e = new IPEndPoint(IPAddress.Any,0);  
            u.BeginReceive(onRecevice, s);
        }

        private void onSend(IAsyncResult res)
        {
            UdpClient u = (UdpClient)((UdpState)(res.AsyncState)).u;
            Console.WriteLine("number of bytes sent: {0}", u.EndSend(res));
        }
    }
}
