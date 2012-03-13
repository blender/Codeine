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

        public PackedDeviceInformations(DeviceInformation[] deviceInfos)
        {
            this.devices = deviceInfos;
            this.count =(byte) deviceInfos.Length;
        }

        public PackedDeviceInformations(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            this.count = (byte)ms.ReadByte();
            this.devices =  new DeviceInformation[this.count];
            int ipStrLen_pos = 2;
            byte ipStrLen = 0;

            for (int i = 0; i < count; i++)
            {
                ipStrLen = data[ipStrLen_pos];
                int sizeOfDeviceInfo = ipStrLen + 2;
                byte[] buff = new byte[sizeOfDeviceInfo + 2];
                ms.Read(buff, 0, sizeOfDeviceInfo);
                this.devices[i] = new DeviceInformation(buff);
                ipStrLen_pos += sizeOfDeviceInfo;
            }
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
