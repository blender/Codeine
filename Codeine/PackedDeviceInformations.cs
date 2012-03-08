using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Codeine
{
    public class PackedDeviceInformations
    {
        public DeviceInformation[] devices;
        public byte count;

        public PackedDeviceInformations(byte count, DeviceInformation[] deviceInfos)
        {
            this.devices = deviceInfos;
            this.count = count;
        }

        public byte[] ToArray() 
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(BitConverter.GetBytes(this.count), 0, sizeof(byte));
            for (int i = 0; i < devices.Length; i++) 
            {
                byte[] bytes = devices[i].ToArray();
                ms.Write(bytes, 0, bytes.Length);
            }

            return ms.ToArray();
        }
    }
}
