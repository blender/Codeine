using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codeine
{
    class CodeineMessageIPs : CodeineMessage
    {
        public PackedDeviceInformations pdi;

        public CodeineMessageIPs(byte subType)
        {
            if (subType == (byte)_t_MSGSUBTYPE.kMSGGetIPs)
            {
                this.msgType = _t_CDMSG.kMSGIPs;
                this.subType = (byte)_t_MSGSUBTYPE.kMSGGetIPs;
            }
            else
                throw new ArgumentException("Can only call this constructor with type _t_MSGGET.kMSGGetContacts");       
        }

        public CodeineMessageIPs(byte subType, PackedDeviceInformations pdi)
        {
            if (pdi == null)
                throw new ArgumentException("PackedDeviceInformations must not be null");

            if (subType == (byte)_t_MSGSUBTYPE.kMSGSetIPs)
            {
                this.msgType = _t_CDMSG.kMSGIPs;
                this.subType = subType;
                this.pdi = pdi;
            }
            else
                throw new ArgumentException("Can only call this constructor with type _t_MSGSET.kMSGSetIPs");
        
        }

        public CodeineMessageIPs(byte[] data) : base(data)
        {

            if (this.msgType != _t_CDMSG.kMSGIPs)
                throw new ArgumentException("Data does not represent a CodeineMessageIPs");
            if (this.subType == (byte)_t_MSGSUBTYPE.kMSGGetIPs) 
            {
                this.pdi = null;
            }
            if (this.subType == (byte)_t_MSGSUBTYPE.kMSGSetIPs) 
            {
                byte[] bytes = new byte[data.Length - 2];

                Array.Copy(data, 2, bytes, 0, data.Length - 2);

                this.pdi = new PackedDeviceInformations(bytes);
            }
        }

        public override byte[] ToArray()
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)this.msgType);
            ms.WriteByte(this.subType);
            if (this.subType == (byte)_t_MSGSUBTYPE.kMSGSetIPs)
            {
                byte[] bytes = this.pdi.ToArray();
                ms.Write(bytes, 0, bytes.Length);
            }
            return ms.ToArray();
        }

    }
}
