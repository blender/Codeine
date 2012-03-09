using System.Runtime.InteropServices;
using System;
using System.IO;

namespace Codeine
{
    public enum _t_MSGSET : byte {ip};
    public enum _t_MSGGET : byte { contacs, ips };
    public enum _t_CDMSG : byte {_t_MSGSET, _t_MSGGET };


    public class CodeineMessage
    {
        public _t_CDMSG msgType;
        public byte subType;
        public byte[] ipAddr;
        public byte cdByteValue;
        public byte ipStrLenght;

        static int sizeOfIpAddr = 16;

        public CodeineMessage(_t_CDMSG msgType, byte subType)
        {
            this.msgType = msgType;
            this.subType = subType;
        }

        public CodeineMessage(_t_CDMSG msgType, byte subType, byte[] ipAddr)
        {
            this.msgType = msgType;
            this.subType = subType;
            this.ipStrLenght = (byte)ipAddr.Length;
            this.ipAddr = ipAddr;
        }

        public CodeineMessage(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            this.msgType = (_t_CDMSG)ms.ReadByte();

            if (msgType == _t_CDMSG._t_MSGGET)
            {
                this.subType =(byte) ms.ReadByte();
            }
            if (msgType == _t_CDMSG._t_MSGSET)
            {
                byte[] buff = new byte[3];
                ms.Read(buff, 0, buff.Length);
                this.subType = buff[0];
                this.cdByteValue = buff[1];
                this.ipStrLenght = buff[2];
                buff = new byte[this.ipStrLenght];
                ms.Read(buff, 0, buff.Length);
                this.ipAddr = buff;
            }
        }

        public byte[] ToByteArray()
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)this.msgType);
            if (this.msgType == _t_CDMSG._t_MSGGET)
            {
                ms.WriteByte(this.subType);
            }
            if (this.msgType == _t_CDMSG._t_MSGSET)
            {
                byte[] buff = new byte[3];
                buff[0] = this.subType;
                buff[1] = this.cdByteValue;
                buff[2] = this.ipStrLenght;
                ms.Write(buff, 0, buff.Length);
                ms.Write(this.ipAddr, 0, ipAddr.Length);
            }

            return ms.ToArray();
        }
    }
}