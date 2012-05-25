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
    public class PackedContactDescriptors
    {
        public byte count;
        public ContactDescriptor[] contacs;

        public PackedContactDescriptors(ContactDescriptor[] descriptors)
        {
            this.contacs = descriptors;
            this.count =(byte) descriptors.Length;
        }

        public PackedContactDescriptors(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            this.count =(byte) ms.ReadByte();

            for (int i = 0; i < count; i++)
            {
                byte[] buff = new byte[ContactDescriptor.size];
                ms.Read(buff, 0, ContactDescriptor.size);
                contacs[i] = new ContactDescriptor(buff);
            }
        }

        public byte[] ToArray() 
        {
            MemoryStream ms = new MemoryStream();
            ms.WriteByte(this.count);
            for (int i = 0; i < count; i++)
            {
                if (contacs[i] != null)
                {
                    ms.Write(contacs[i].ToArray(), 0, ContactDescriptor.size);
                }
            }

            return ms.ToArray();
        }

    }
}
