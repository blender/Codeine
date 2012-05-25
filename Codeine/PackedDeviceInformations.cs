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
