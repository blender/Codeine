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

            byte[] dataToSend = new byte[0];
            UdpClient u = (UdpClient)((UdpState)(res.AsyncState)).u;
            IPEndPoint e = (IPEndPoint)((UdpState)(res.AsyncState)).e;

            UdpState s = new UdpState();
            s.e = e;
            s.u = u;

            byte[] data = u.EndReceive(res, ref e);

            CodeineMessage cdMsg = new CodeineMessage(data);
            if (cdMsg.msgType == _t_CDMSG._t_MSGGET)
            {
                if (cdMsg.subType == (byte)_t_MSGGET.contacs)
                {
                    PackedContactDescriptors descs = cDataController.packedDescriptors;
                    dataToSend = StructConverter.ToByteArray(descs);
                }
                if (cdMsg.subType == (byte)_t_MSGGET.ips)
                {
                    PackedDeviceInformations packedDeviceInfos = cDataController.packedDeviceInfos;
                    dataToSend = packedDeviceInfos.ToArray();
                }

                u.BeginSend(dataToSend, dataToSend.Length, e, onSend, s);
            }
            if (cdMsg.msgType == _t_CDMSG._t_MSGSET) 
            {
                string ipStr = Encoding.ASCII.GetString(cdMsg.ipAddr);
                Console.WriteLine(ipStr);
                cDataController.updateDeviceInfo(new DeviceInformation(cdMsg.cdByteValue, ipStr)); 

            }

            s.e = new IPEndPoint(IPAddress.Any, 0);
            u.BeginReceive(onRecevice, s);
        }

        private void onSend(IAsyncResult res)
        {
            UdpClient u = (UdpClient)((UdpState)(res.AsyncState)).u;
            Console.WriteLine("number of bytes sent: {0}", u.EndSend(res));
        }
    }
}
