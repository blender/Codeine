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
        public string ipAddr;

        public DeviceInformation(byte cdByteValue, string ipAddr) 
        {
            this.contactDescriptorByteValue = cdByteValue;
            this.ipAddr = ipAddr;
        }

        public byte[] ToArray() 
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte((byte)this.contactDescriptorByteValue);
            byte[] bytes = Encoding.ASCII.GetBytes(this.ipAddr);
            ms.Write(bytes, 0, bytes.Length);

            return ms.ToArray();
        }
    }
}
