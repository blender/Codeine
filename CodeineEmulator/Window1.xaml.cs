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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using Codeine;

namespace CodeineEmulator
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private delegate void SeverThread();
        
        private Point origin;
        private Point start;
        public volatile EmulatorContactDataController contactDataController;
        public static EmulatorServerController serverController;

        private static byte iPhoneID = 9;
        private static byte iPadID = 0;

        Image img;
       
        public Window1()
        {
            InitializeComponent();
            TransformGroup group = new TransformGroup();

            TranslateTransform tt = new TranslateTransform();
            group.Children.Add(tt);

            image1.RenderTransform = group;

            TransformGroup group2 = new TransformGroup();

            TranslateTransform tt2 = new TranslateTransform();
            group2.Children.Add(tt2);
            image2.RenderTransform = group2;

            contactDataController = EmulatorContactDataController.Instance;

            Point p1 = new Point(12, 39);
            ContactDescriptor c1 = new ContactDescriptor(iPhoneID,(int) p1.X,(int) p1.Y, 180.0f);
            contactDataController.addContact(iPhoneID, c1);

            Point p2 = new Point(245, 190);
            ContactDescriptor c2 = new ContactDescriptor(iPadID, (int)p2.X, (int)p2.Y, 180.0f);
            contactDataController.addContact(iPadID, c2);

            //serverController = new ServerController(5);
            //Thread oThread = new Thread(new ThreadStart(serverController.startServer));
            SeverThread fetcher = new SeverThread(this.runServer);
            //oThread.Start();
            fetcher.BeginInvoke(null, null);
        }

        private void runServer()
        {
            /*
            int i = 0;
            while (true)
            {
                i++;
                int res = i % 1000;
                if (res == 0)
                {
                    Console.WriteLine("Hello!");
                    i = 1;
                }
            }
             */
            serverController = EmulatorServerController.Instance;
        }

        private void image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            img = (Image)sender;
            img.ReleaseMouseCapture();
        }

        private void image_MouseMove(object sender, MouseEventArgs e)
        {
            img = (Image)sender;
            if (!img.IsMouseCaptured) return;

            var tt = (TranslateTransform)((TransformGroup)img.RenderTransform).Children.First(tr => tr is TranslateTransform);
            Vector v = start - e.GetPosition(grid1);
            tt.X = origin.X - v.X;
            tt.Y = origin.Y - v.Y;

            if (img == image1) 
            {
                Vector v1 = new Vector(e.GetPosition(grid1).X, e.GetPosition(grid1).Y);
                Point p = new Point(v1.X, v1.Y);
                ContactDescriptor c = new ContactDescriptor(iPhoneID, (int)p.X, (int)p.Y, 180.0f);
                contactDataController.updateContact(iPhoneID, c);
            }
            if (img == image2)
            {

                Vector v2 = new Vector(e.GetPosition(grid1).X, e.GetPosition(grid1).Y);
                Point p = new Point(v2.X, v2.Y);
                ContactDescriptor c = new ContactDescriptor(iPadID, (int)p.X, (int)p.Y, 180.0f);
                contactDataController.updateContact(iPadID, c);
              }
        }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            img.CaptureMouse();
            var tt = (TranslateTransform)((TransformGroup)img.RenderTransform).Children.First(tr => tr is TranslateTransform);
            start = e.GetPosition(grid1);
            origin = new Point(tt.X, tt.Y);
        }

        private void image2_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}
