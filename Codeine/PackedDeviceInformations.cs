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
            ms.WriteByte((byte)devices.Length);
            for (int i = 0; i < devices.Length; i++) 
            {
                if (devices[i] != null)
                {
                    byte[] bytes = devices[i].ToArray();
                    ms.Write(bytes, 0, bytes.Length);
                }
            }

            return ms.ToArray();
        }
    }
}
