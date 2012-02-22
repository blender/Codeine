using System.Runtime.InteropServices;
using System;
using System.IO;
namespace Codeine
{
    
    [Serializable]
    public struct ContactDescriptor
    {
        public ContactDescriptor(byte byteVal, double posX, double posY, double orient)
        {
            byteValue = byteVal;
            positionX = posX;
            positionY = posY;
            orientation = orient;
        }

        public byte byteValue;
        public double positionX;
        public double positionY;
        public double orientation;
    }

    //[StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct PackedContactDescriptors
    {
        public PackedContactDescriptors(byte cnt, ContactDescriptor[] arr)
        {
            count = cnt;
            descArray = arr;
        }

        public byte count;
        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public ContactDescriptor[] descArray;

    }

    public static class StructConverter 
    {
        public static byte[] ToByteArray(PackedContactDescriptors pcd)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(BitConverter.GetBytes(pcd.count), 0, sizeof(byte));
            for (int i = 0; i < pcd.count; i++) {

                ms.Write(ToByteArray(pcd.descArray[i]), 0, sizeofCD);
            }

            return ms.ToArray();
        }

        public static byte[] ToByteArray(ContactDescriptor cd)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(BitConverter.GetBytes(cd.byteValue), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes(cd.orientation), 0, sizeof(double));
            ms.Write(BitConverter.GetBytes(cd.positionX), 0, sizeof(double));
            ms.Write(BitConverter.GetBytes(cd.positionY), 0, sizeof(double));

            return ms.ToArray();
        }
        private static int sizeofCD = sizeof(byte) + 3 * sizeof(double);
        public static PackedContactDescriptors fromArray(byte[] data)
        {
            PackedContactDescriptors pcd = new PackedContactDescriptors();
            MemoryStream ms = new MemoryStream(data);
            pcd.count =  (byte) ms.ReadByte();
            pcd.descArray = new ContactDescriptor[pcd.count];

            for (int i = 0; i < pcd.count; i++)
            {
                byte[] buff = new byte[sizeofCD];
                ms.Read(buff, 0,sizeofCD);
                pcd.descArray[i] = CDfromArray(buff);
            }
            return pcd;
        }

        private static ContactDescriptor CDfromArray(byte[] buff)
        {
            
            ContactDescriptor cd = new ContactDescriptor();
            
            cd.byteValue = buff[0];
            int s = 0;
            s += sizeof(byte);
            cd.orientation = BitConverter.ToDouble(buff,  s);
            s += sizeof(double);
            cd.positionX = BitConverter.ToDouble(buff, s);

            s += sizeof(double);
            cd.positionY = BitConverter.ToDouble(buff, s);
            return cd;
        }
    }
}