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
    public class ContactDescriptor
    {
        byte byteValue;
        int positionX;
        int positionY;
        float orientation;

        public static int size = sizeof(byte) + 3 * sizeof(int);

        public ContactDescriptor(byte byteValue, int positionX, int positionY, float orientation)
        {
            this.byteValue = byteValue;
            this.positionX = positionX;
            this.positionY = positionY;
            this.orientation = orientation;
        }

        public ContactDescriptor(byte[] data) {

            this.byteValue = data[0];
            int s = 1;
            this.positionX = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, s));

            s += sizeof(int);
            this.positionY = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, s));

            s += sizeof(int);
            int angle = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(data, s));
            this.orientation = angle / 10.0f;
        }

        public byte[] ToArray() 
        {
            MemoryStream ms = new MemoryStream();

            ms.WriteByte(this.byteValue);
            ms.Write(BitConverter.GetBytes(this.positionX), 0, sizeof(int));
            ms.Write(BitConverter.GetBytes(this.positionY), 0, sizeof(int));
            int orientation =(int)( this.orientation * 10.0f);
            ms.Write(BitConverter.GetBytes(orientation), 0, sizeof(int));

            return ms.ToArray();
        }

    }
}
