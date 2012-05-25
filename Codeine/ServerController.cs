//  Copyright (c) 2012 Tommaso Piazza <tommaso.piazza@gmail.com>
//
//  This file is part of Codeine.
//
//  Codeine is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Codeine is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with Codeine.  If not, see <http://www.gnu.org/licenses/>.


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


   
    public class ServerController
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

            if (cdMsg.msgType == _t_CDMSG.kMSGContacts)
            {
                if (cdMsg.subType == (byte) _t_MSGSUBTYPE.kMSGGetContacs)
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
