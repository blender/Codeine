﻿using System;
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
        public static EmulatorContactDataController contactDataController;
        public static ServerController serverController;

        private static byte iPhoneID = 6;
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

            Vector v1 =  VisualTreeHelper.GetOffset(image1);
            Point p1 = new Point(v1.X, v1.Y);
            ContactDescriptor c1 = new ContactDescriptor(iPhoneID,(int) p1.X,(int) p1.Y, 180.0f);
            contactDataController.addContact(iPhoneID, c1);

            Vector v2 = VisualTreeHelper.GetOffset(image2);
            Point p2 = new Point(v2.X, v2.Y);
            ContactDescriptor c2 = new ContactDescriptor(iPadID, (int)p2.X, (int)p2.Y, 180.0f);
            contactDataController.addContact(iPadID, c2);

            SeverThread fetcher = new SeverThread(this.runServer);

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
            serverController = ServerController.Instance;
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