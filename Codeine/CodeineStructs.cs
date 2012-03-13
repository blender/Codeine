using System.Runtime.InteropServices;
using System;
using System.IO;
namespace Codeine
{
    /*
    [Serializable]
    public struct ContactDescriptor
    {
        public ContactDescriptor(byte byteVal, int posX, int posY, int orient)
        {
            byteValue = byteVal;
            _pad8 = byteVal;
            _pad16 = byteVal;
            _pad32 = byteVal;
            positionX = posX;
            positionY = posY;
            orientation = orient;
        }

        public byte byteValue;
        public byte _pad8;
        public byte _pad16;
        public byte _pad32;
        public int positionX;
        public int positionY;
        public int orientation;
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
    */

    /*
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
            ms.Write(BitConverter.GetBytes((cd.byteValue)), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes((cd._pad8)), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes((cd._pad16)), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes((cd._pad32)), 0, sizeof(byte));
            ms.Write(BitConverter.GetBytes((cd.positionX)), 0, sizeof(int));
            ms.Write(BitConverter.GetBytes((cd.positionY)), 0, sizeof(int));
            ms.Write(BitConverter.GetBytes((cd.orientation)), 0, sizeof(int));
            

            return ms.ToArray();
        }
        private static int sizeofCD = 4*sizeof(byte) + 3 * sizeof(int);
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
            cd._pad8 = buff[1];
            cd._pad16 = buff[2];
            cd._pad32 = buff[3];
            int s = 0;
            s += sizeof(int);
            cd.positionX = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buff, s));

            s += sizeof(int);
            cd.positionY = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buff, s));

            s += sizeof(int);
            cd.orientation = System.Net.IPAddress.NetworkToHostOrder(BitConverter.ToInt32(buff, s));

            return cd;
        }
    }
     */
}
