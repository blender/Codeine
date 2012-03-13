using System.Runtime.InteropServices;
using System;
using System.IO;

namespace Codeine
{
    public enum _t_MSGSUBTYPE : byte {kMSGSetContacts, kMSGSetIPs, kMSGGetContacs, kMSGGetIPs};
    public enum _t_CDMSG : byte {kMSGContacts, kMSGIPs };


    public class CodeineMessage
    {
        public _t_CDMSG msgType;
        public byte subType;

        public CodeineMessage() { }

        public CodeineMessage(_t_CDMSG msgType, byte subType)
        {
            this.msgType = msgType;
            this.subType = subType;
        }

        public CodeineMessage(byte[] data) : this((_t_CDMSG)data[0], (byte)data[1])
        { 
        }

        public virtual byte[] ToArray()
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)this.msgType);
            ms.WriteByte(this.subType);
            return ms.ToArray();
        }
    }
}