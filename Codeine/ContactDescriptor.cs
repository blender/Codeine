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
