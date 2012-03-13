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
