//  Copyright (c) 2012 Tommaso Piazza <tommaso.piazza@gmail.com>
//
//  This file is part of CodeineEmulator.
//
//  CodeineEmulator is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  CodeineEmulator is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with CodeineEmulator.  If not, see <http://www.gnu.org/licenses/>.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Codeine;

namespace CodeineEmulator
{
    public class EmulatorContactDataController
    {
        private static EmulatorContactDataController instance;
        public Dictionary<byte, ContactDescriptor> contactDictionary;
        public Dictionary<byte, DeviceInformation> deviceInfoDictionary;

        private EmulatorContactDataController()
        {
            contactDictionary = new Dictionary<byte, ContactDescriptor>();
            deviceInfoDictionary = new Dictionary<byte, DeviceInformation>();
        }

        public static EmulatorContactDataController Instance
        {
            get
            {

                if (instance == null)
                {

                    instance = new EmulatorContactDataController();

                }

                return instance;
            }
        }

        public void addContact(byte tagByteValue, ContactDescriptor c)
        {
                contactDictionary.Add(tagByteValue, c);
                try
                {
                    deviceInfoDictionary.Add(tagByteValue, new DeviceInformation(tagByteValue, ""));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Device With Byte Value %d already in deviceInfoDictionary", tagByteValue);
                    Console.WriteLine(e.ToString());
                }
        }

        public void updateContact(byte tagByteValue, ContactDescriptor c)
        {
                if (contactDictionary.Remove(tagByteValue))
                {
                    contactDictionary.Add(tagByteValue, c);
                }
        }

        public void removeContact(byte tagByteValue)
        {

            contactDictionary.Remove(tagByteValue);
            deviceInfoDictionary.Remove(tagByteValue);

        }

        public PackedContactDescriptors packedDescriptors
        {
            get
            {
                ContactDescriptor[] cds = new ContactDescriptor[contactDictionary.Count];
                int i = 0;
                foreach (KeyValuePair<byte, ContactDescriptor> k in contactDictionary)
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
