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