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
