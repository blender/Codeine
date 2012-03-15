using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Codeine;
using System.IO;


namespace CodeineEmulator
{
    public struct UdpState
    {
        public UdpClient u { get; set; }
        public IPEndPoint e { get; set; }
    }



    public class EmulatorServerController
    {
        private static EmulatorServerController instance;

        private const int udpPort = 4568;
        private UdpClient uClient;
        private EmulatorContactDataController cDataController;

        private EmulatorServerController()
        {
            IPEndPoint e = new IPEndPoint(IPAddress.Any, udpPort);
            uClient = new UdpClient(udpPort);
            cDataController = EmulatorContactDataController.Instance;

            UdpState s = new UdpState();
            s.e = e;
            s.u = uClient;

            uClient.BeginReceive(new AsyncCallback(onRecevice), s);
        }

        public static EmulatorServerController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EmulatorServerController();
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

            if (cdMsg.msgType == _t_CDMSG.kMSGContacts)
            {
                if (cdMsg.subType == (byte)_t_MSGSUBTYPE.kMSGGetContacs)
                {
                    PackedContactDescriptors descs = cDataController.packedDescriptors;
                    CodeineMessageContacs cmC = new CodeineMessageContacs((byte)_t_MSGSUBTYPE.kMSGSetContacts, descs);
                    dataToSend = cmC.ToArray();
                    u.BeginSend(dataToSend, dataToSend.Length, e, onSend, s);
                }
            }
            if (cdMsg.msgType == _t_CDMSG.kMSGIPs)
            {
                if (cdMsg.subType == (byte)_t_MSGSUBTYPE.kMSGGetIPs)
                {
                    PackedDeviceInformations packedDeviceInfos = cDataController.packedDeviceInfos;
                    CodeineMessageIPs cmIPs = new CodeineMessageIPs((byte)_t_MSGSUBTYPE.kMSGSetIPs, packedDeviceInfos);
                    dataToSend = cmIPs.ToArray();
                    u.BeginSend(dataToSend, dataToSend.Length, e, onSend, s);
                }
                if (cdMsg.subType == (byte)_t_MSGSUBTYPE.kMSGSetIPs)
                {
                    CodeineMessageIPs cmIPs = new CodeineMessageIPs(data);
                    PackedDeviceInformations pdi = cmIPs.pdi;
                    cDataController.updateDeviceInfo(pdi.devices[0]);
                }
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
