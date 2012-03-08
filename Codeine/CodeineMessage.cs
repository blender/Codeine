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

        static int sizeOfGetMessage = 4;
        static int sizeOfMsgTypeWithPreamble = 4;
        static int sizeOfIpAddr = 16;
        static int sizeOfSetMessage = 4 + 16;

        public CodeineMessage(_t_CDMSG msgType, byte subType)
        {
            this.msgType = msgType;
            this.subType = subType;
        }

        public CodeineMessage(_t_CDMSG msgType, byte subType, byte[] ipAddr)
        {
            this.msgType = msgType;
            this.subType = subType;
            this.ipAddr = ipAddr;
        }

        public CodeineMessage(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            this.msgType = (_t_CDMSG)ms.ReadByte();
            ms.ReadByte();
            ms.ReadByte();
            ms.ReadByte();
            if (msgType == _t_CDMSG._t_MSGGET)
            {
                byte[] buff = new byte[sizeOfGetMessage];
                ms.Read(buff, 0, buff.Length);
                this.subType = buff[0];
            }
            if (msgType == _t_CDMSG._t_MSGSET)
            {
                byte[] buff = new byte[sizeOfMsgTypeWithPreamble];
                ms.Read(buff, 0, buff.Length);
                this.subType = buff[0];
                this.cdByteValue = buff[1];
                buff = new byte[sizeOfIpAddr];
                ms.Read(buff, 0, buff.Length);
                this.ipAddr = buff;


            }
        }

        /*
    
        [Serializable]
        public unsafe struct CodeineMessage
        {
            unsafe public CodeineMessage(_t_CDMSG mType, void* pload)
            {
                msgType = mType;
                _pad8 = (byte)mType;
                _pad16 = (byte)mType;
                _pad32 = (byte)mType;
                payload = pload;
            }

            public _t_CDMSG msgType;
            public byte _pad8;
            public byte _pad16;
            public byte _pad32;
            public void *payload;
        }

        [Serializable]
        public struct GetMessage
        {
            public GetMessage(_t_MSGGET mType)
            {
                msgType = mType;
                _pad8 = (byte)mType;
                _pad16 = (byte)mType;
                _pad32 = (byte)mType;

            }

            public _t_MSGGET msgType;
            public byte _pad8;
            public byte _pad16;
            public byte _pad32;
        }

        [Serializable]
        public unsafe struct SetMessage
        {
            public SetMessage(_t_MSGSET mType, byte cdByteValue, byte[] ipAddr)
            {
                msgType = mType;
                contactDesctiptorByteValue = cdByteValue;
                fixed(byte* ip = ipAddr)
                _pad16 = (byte)cdByteValue;
                _pad32 = (byte)cdByteValue;
            }

            public _t_MSGSET msgType;
            public byte contactDesctiptorByteValue;
            public byte _pad16;
            public byte _pad32;
            public fixed byte ip[16];
        }
        */
        public byte[] ToByteArray()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(BitConverter.GetBytes((byte)this.msgType), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes((byte)this.msgType), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes((byte)this.msgType), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes((byte)this.msgType), 0, sizeof(byte));
            if (this.msgType == _t_CDMSG._t_MSGGET)
            {

                byte[] buff = new byte[sizeOfGetMessage];
                buff[0] = buff[1] = buff[2] = buff[3] = this.subType;
                ms.Write(buff, 0, buff.Length);
            }
            if (this.msgType == _t_CDMSG._t_MSGSET)
            {
                byte[] buff = new byte[sizeOfMsgTypeWithPreamble];
                buff[3] = buff[2] = buff[0] = this.subType;
                buff[1] = this.cdByteValue;
                ms.Write(buff, 0, buff.Length);
                ms.Write(this.ipAddr, 0, ipAddr.Length);
            }

            return ms.ToArray();
        }
        /*
        public static unsafe byte[] ToByteArray(GetMessage getMsg)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(BitConverter.GetBytes((byte)getMsg.msgType), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes(getMsg._pad8), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes(getMsg._pad16), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes(getMsg._pad32), 0, sizeof(byte));
            return ms.ToArray();
        }


        public static unsafe byte[] ToByteArray(SetMessage setMsg)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(BitConverter.GetBytes((byte)setMsg.msgType), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes(setMsg.contactDesctiptorByteValue), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes(setMsg._pad16), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes(setMsg._pad32), 0, sizeof(byte));
            for (int i = 0; i < 16; i++)
            {
                byte toWrite = (byte)setMsg.ip[i];
                ms.Write(BitConverter.GetBytes(toWrite), 0, sizeof(byte));
            }
            return ms.ToArray();
        }


        public static unsafe CodeineMessage CodeineMessagefromArray(byte[] data)
        {
            CodeineMessage cdMsg = new CodeineMessage();
            MemoryStream ms = new MemoryStream(data);
            cdMsg.msgType = (_t_CDMSG)ms.ReadByte();
            ms.ReadByte();
            ms.ReadByte();
            ms.ReadByte();
            if (cdMsg.msgType == _t_CDMSG._t_MSGGET) {
                byte[] buff = new byte[sizeof(GetMessage)];
                ms.Read(buff, 0, buff.Length);
                GetMessage msg = GetMessageFromByteArray(buff);
                cdMsg.payload =(void *) &msg;
            }
            if (cdMsg.msgType == _t_CDMSG._t_MSGSET) {
                _t_MSGSET setType = (_t_MSGSET) ms.ReadByte();
                byte cdByteValue =(byte) ms.ReadByte();
                ms.ReadByte();
                ms.ReadByte();
                byte[] buff = new byte[16];
                ms.Read(buff, 0, 16);
                SetMessage msg  = new SetMessage(setType, cdByteValue, buff);
                //FIXME: Something is wrong here econdedIP does not get copied
                //and the correspoind field of the structure must be filled manually
                for (int k = 0; k < buff.Length; k++)
                {
                    msg.ip[k] = buff[k];
                }
                msg.ip[15] = (byte)'\0';
                cdMsg.payload =(void *) &msg;

            
            }
            
            return cdMsg;
        }

        private static GetMessage GetMessageFromByteArray(byte[] data)
        {
            GetMessage gMesg = new GetMessage((_t_MSGGET)data[0]);

            return gMesg;
        }
        */
    }
}