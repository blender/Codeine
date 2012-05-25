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
