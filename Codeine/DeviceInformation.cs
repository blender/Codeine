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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codeine
{
    public class DeviceInformation
    {
        public byte contactDescriptorByteValue;
        public byte ipStrLength;
        public string ipAddr;

        public DeviceInformation(byte cdByteValue, string ipAddr) 
        {
            this.contactDescriptorByteValue = cdByteValue;
            this.ipStrLength =(byte) ipAddr.Length;
            this.ipAddr = ipAddr;
        }

        public DeviceInformation(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);

            this.contactDescriptorByteValue =(byte) ms.ReadByte();
            this.ipStrLength = (byte)ms.ReadByte();
            byte[] buff = new byte[this.ipStrLength];
            ms.Read(buff, 0, this.ipStrLength);
            this.ipAddr = Encoding.ASCII.GetString(buff);

        }

        public byte[] ToArray() 
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)this.contactDescriptorByteValue);
            ms.WriteByte((byte)this.ipStrLength);
            byte[] bytes = Encoding.ASCII.GetBytes(this.ipAddr);
            ms.Write(bytes, 0, bytes.Length);

            return ms.ToArray();
        }
    }
}
