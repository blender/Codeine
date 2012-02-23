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

        private ContacDataController()
        {

            contactDictionary = new Dictionary<Contact, ContactDescriptor>();
        
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
                   desc = new ContactDescriptor(c.Tag.Byte.Value, (int)p.X, (int)p.Y, (int)(c.GetOrientation(parentGrid)*10.0));
               }
           }
       }
       
       public void removeContact(Contact c)
       {
           if (c.Tag.Type == TagType.Byte)
           {
                   contactDictionary.Remove(c);
           }
       }

       public PackedContactDescriptors packedDescriptors
       {
           get
           {
              ContactDescriptor[] cds = new ContactDescriptor[10];
              int i = 0;
              foreach(KeyValuePair<Contact, ContactDescriptor> k in contactDictionary)
              {
                  cds[i++] = k.Value;             
              }

              return new PackedContactDescriptors((byte)contactDictionary.Count, cds);
           }
       }
     }
}
