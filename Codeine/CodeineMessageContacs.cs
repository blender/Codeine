using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codeine
{
    public class CodeineMessageContacs : CodeineMessage
    {
        public PackedContactDescriptors pcd;

        public CodeineMessageContacs(byte subType)
        {
            if (subType == (byte)_t_MSGSUBTYPE.kMSGGetContacs)
            {
                this.msgType = _t_CDMSG.kMSGContacts;
                this.subType = (byte)_t_MSGSUBTYPE.kMSGGetContacs;
            }
            else
                throw new ArgumentException("Can only call this constructor with type _t_MSGGET.kMSGGetContacts");
                
        }

        public CodeineMessageContacs(byte subType, PackedContactDescriptors pcd)
        {
            if (pcd == null)
                throw new ArgumentException("PackedContactDescriptors must not be null");

            if (subType == (byte)_t_MSGSUBTYPE.kMSGSetContacts)
            {
                this.msgType = _t_CDMSG.kMSGContacts;
                this.subType = subType;
                this.pcd = pcd;
            }
            else
                throw new ArgumentException("Can only call this constructor with type _t_MSGGET.kMSGSetContacts");
        
        }

        public CodeineMessageContacs(byte[] data):base(data)
        {

            if (this.msgType != (byte)_t_CDMSG.kMSGContacts)
                throw new ArgumentException("Data does not represent a CodeineMessageContacts");
            if (this.subType == (byte)_t_MSGSUBTYPE.kMSGGetContacs) 
            {
                this.pcd = null;
            }
            if (this.subType == (byte)_t_MSGSUBTYPE.kMSGSetContacts) 
            {
                byte[] bytes = new byte[data.Length - 2];

                Array.Copy(data, 2, bytes, 0, data.Length - 2);

                this.pcd = new PackedContactDescriptors(bytes);
            }

        }

        public override byte[] ToArray() 
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)this.msgType);
            ms.WriteByte(this.subType);
            if (this.subType == (byte)_t_MSGSUBTYPE.kMSGSetContacts)
            {
                byte[] bytes = this.pcd.ToArray();
                ms.Write(bytes, 0, bytes.Length);
            }
            return ms.ToArray();
        }

    }
}
