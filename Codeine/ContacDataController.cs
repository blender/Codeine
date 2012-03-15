using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Surface.Presentation;

namespace Codeine
{      
    public class ContacDataController
    {

        private static ContacDataController instance;
        private static Dictionary<Contact, ContactDescriptor> contactDictionary;
        private static Dictionary<byte, DeviceInformation> deviceInfoDictionary;

        private ContacDataController()
        {
            contactDictionary = new Dictionary<Contact, ContactDescriptor>();
            deviceInfoDictionary = new Dictionary<byte, DeviceInformation>();
        }

        public static ContacDataController Instance
        {
            get
            {

                if (instance == null)
                {

                    instance = new ContacDataController();

                }

                return instance;
            }
        }

       public void addContact(Contact c, Grid parentGrid)
       {
            if(c.Tag.Type == TagType.Byte)
            {
                Point p = c.GetPosition(parentGrid);
                double orientation = c.GetOrientation(parentGrid);
                ContactDescriptor desc = new ContactDescriptor(c.Tag.Byte.Value, (int)p.X, (int) p.Y,(int) (orientation*10.0));
                contactDictionary.Add(c, desc);
                try
                {
                    deviceInfoDictionary.Add(c.Tag.Byte.Value, new DeviceInformation(c.Tag.Byte.Value, ""));
                }catch (Exception e) {
                    Console.WriteLine("Device With Byte Value %d already in deviceInfoDictionary", c.Tag.Byte.Value);
                    Console.WriteLine(e.ToString());
                }
            }
       }

       public void updateContact(Contact c, Grid parentGrid) 
       {

           if (c.Tag.Type == TagType.Byte)
           {
               ContactDescriptor desc;
               if (contactDictionary.Remove(c))
               {
                   Point p = c.GetPosition(parentGrid);
                   desc = new ContactDescriptor(c.Tag.Byte.Value, (int)p.X, (int)p.Y, (int)(c.GetOrientation(parentGrid)));
                   contactDictionary.Add(c, desc);
               }
           }
       }
       
       public void removeContact(Contact c)
       {
           if (c.Tag.Type == TagType.Byte)
           {
               contactDictionary.Remove(c);
               deviceInfoDictionary.Remove(c.Tag.Byte.Value); 
           }
       }

       public PackedContactDescriptors packedDescriptors
       {
           get
           {
              ContactDescriptor[] cds = new ContactDescriptor[contactDictionary.Count];
              int i = 0;
              foreach(KeyValuePair<Contact, ContactDescriptor> k in contactDictionary)
              {
                  if (i < contactDictionary.Count)
                  {
                      cds[i++] = k.Value;
                  }
              }

              return new PackedContactDescriptors(cds);
           }
       }

       public PackedDeviceInformations packedDeviceInfos 
       {
           get 
           {
               DeviceInformation[] dInfos = new DeviceInformation[deviceInfoDictionary.Count];
               int i = 0;
               foreach (KeyValuePair<byte, DeviceInformation> k in deviceInfoDictionary)
               {
                   if (i < deviceInfoDictionary.Count)
                   {
                       dInfos[i++] = k.Value;
                   }
               }

               return new PackedDeviceInformations(dInfos);
           }       
       }

       public void updateDeviceInfo(DeviceInformation deviceInfo) 
       {
           deviceInfoDictionary.Remove(deviceInfo.contactDescriptorByteValue);
           deviceInfoDictionary.Add(deviceInfo.contactDescriptorByteValue, deviceInfo);
       }
     }
}
