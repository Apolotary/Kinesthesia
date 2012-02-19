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
using Kinesthesia.UI_Controllers;
using Microsoft.Kinect;
using Kinesthesia.Model.MIDI;
using Coding4Fun.Kinect.Wpf; 

namespace Kinesthesia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool closing = false;
        const int skeletonCount = 6;
        MidiManager midMan = new MidiManager();
        Skeleton[] allSkeletons = new Skeleton[skeletonCount];

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
            UpdateNoteRectangles();
        }
        
        private void UpdateNoteRectangles()
        {
            noteC.InsertNoteData("C", 6, 83, 0.9, 1.0, Brushes.DarkRed, Brushes.Green);
            noteD.InsertNoteData("F", 6, 83, 0.9, 1.0, Brushes.DarkRed, Brushes.Green);
            noteE.InsertNoteData("D#", 6, 83, 0.9, 1.0, Brushes.DarkRed, Brushes.Green);
            noteF.InsertNoteData("G#", 6, 83, 0.9, 1.0, Brushes.DarkRed, Brushes.Green);
            noteG.InsertNoteData("A#", 6, 83, 0.9, 1.0, Brushes.DarkRed, Brushes.Green);
            noteA.InsertNoteData("C", 7, 83, 0.9, 1.0, Brushes.DarkRed, Brushes.Green);
            noteB.InsertNoteData("B", 6, 83, 0.9, 1.0, Brushes.DarkRed, Brushes.Green);

            noteC.RectangleTriggered += new EventHandler(SendNoteOn);
            noteC.RectangleUntriggered += new EventHandler(SendNoteOff);

            noteD.RectangleTriggered += new EventHandler(SendNoteOn);
            noteD.RectangleUntriggered += new EventHandler(SendNoteOff);

            noteE.RectangleTriggered += new EventHandler(SendNoteOn);
            noteE.RectangleUntriggered += new EventHandler(SendNoteOff);

            noteF.RectangleTriggered += new EventHandler(SendNoteOn);
            noteF.RectangleUntriggered += new EventHandler(SendNoteOff);

            noteG.RectangleTriggered += new EventHandler(SendNoteOn);
            noteG.RectangleUntriggered += new EventHandler(SendNoteOff);

            noteA.RectangleTriggered += new EventHandler(SendNoteOn);
            noteA.RectangleUntriggered += new EventHandler(SendNoteOff);

            noteB.RectangleTriggered += new EventHandler(SendNoteOn);
            noteB.RectangleUntriggered += new EventHandler(SendNoteOff);
        }

        private void kinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor old = (KinectSensor)e.OldValue;

            StopKinect(old);

            KinectSensor sensor = (KinectSensor)e.NewValue;

            if (sensor == null)
            {
                return;
            }

            sensor.SkeletonStream.Enable();

            sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);
            sensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
            sensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);

            try
            {
                sensor.Start();
            }
            catch (System.IO.IOException)
            {
                kinectSensorChooser1.AppConflictOccurred();
            }
        }

        private void SendNoteOn (object sender, EventArgs e)
        {
            NoteRectangleControl noteRect = (NoteRectangleControl) sender;
            NoteEventArgs ee = (NoteEventArgs) e;

            noteRect.FillRectangle();
            noteRect.SwitchSignal();

            midMan.SendNoteOnMessage(ee.note, ee.octave, ee.velocity);
        }

        private void SendNoteOff(object sender, EventArgs e)
        {
            NoteRectangleControl noteRect = (NoteRectangleControl)sender;
            NoteEventArgs ee = (NoteEventArgs)e;

            noteRect.UnFillRectangle();
            noteRect.SwitchSignal();

            midMan.SendNoteOffMessage(ee.note, ee.octave, ee.velocity);
        }

        private void StopKinect(KinectSensor sensor)
        {
            if (sensor != null)
            {
                if (sensor.IsRunning)
                {
                    //stop sensor 
                    sensor.Stop();

                    //stop audio if not null
                    if (sensor.AudioSource != null)
                    {
                        sensor.AudioSource.Stop();
                    }
                }
            }
        }

        void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            if (closing)
            {
                return;
            }

            //Get a skeleton
            Skeleton first = GetFirstSkeleton(e);

            if (first == null)
            {
                return;
            }


            GetCameraPoint(first, e);

        }

        void GetCameraPoint(Skeleton first, AllFramesReadyEventArgs e)
        {

            using (DepthImageFrame depth = e.OpenDepthImageFrame())
            {
                if (depth == null ||
                    kinectSensorChooser1.Kinect == null)
                {
                    return;
                }

                Joint left = first.Joints[JointType.HandLeft];
                Joint right = first.Joints[JointType.HandRight];

                //Map a joint location to a point on the depth map
                //head
                DepthImagePoint headDepthPoint =
                    depth.MapFromSkeletonPoint(first.Joints[JointType.Head].Position);
                //left hand
                DepthImagePoint leftDepthPoint =
                    depth.MapFromSkeletonPoint(left.Position);
                //right hand
                DepthImagePoint rightDepthPoint =
                    depth.MapFromSkeletonPoint(right.Position);


                //Map a depth point to a point on the color image
                //head
                ColorImagePoint headColorPoint =
                    depth.MapToColorImagePoint(headDepthPoint.X, headDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //left hand
                ColorImagePoint leftColorPoint =
                    depth.MapToColorImagePoint(leftDepthPoint.X, leftDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);
                //right hand
                ColorImagePoint rightColorPoint =
                    depth.MapToColorImagePoint(rightDepthPoint.X, rightDepthPoint.Y,
                    ColorImageFormat.RgbResolution640x480Fps30);

                Point rightPoint = new Point(rightColorPoint.X, rightColorPoint.Y);
                Point leftPoint = new Point(leftColorPoint.X, leftColorPoint.Y);

                Point[] points = {leftPoint, rightPoint};

                label2.Content = leftPoint.X;
                label3.Content = leftPoint.Y;
                label5.Content = rightPoint.X;
                label6.Content = rightPoint.Y;

                noteC.CheckIntersectionWithPoints(leftPoint);
                //noteC.CheckIntersectionWithPoints(rightPoint);
                noteD.CheckIntersectionWithPoints(leftPoint);
                //noteD.CheckIntersectionWithPoints(rightPoint);
                noteE.CheckIntersectionWithPoints(leftPoint);
                //noteE.CheckIntersectionWithPoints(rightPoint);
                noteF.CheckIntersectionWithPoints(leftPoint);
                //noteF.CheckIntersectionWithPoints(rightPoint);
                noteG.CheckIntersectionWithPoints(leftPoint);
                //noteG.CheckIntersectionWithPoints(rightPoint);
                noteA.CheckIntersectionWithPoints(leftPoint);
                //noteA.CheckIntersectionWithPoints(rightPoint);
                noteB.CheckIntersectionWithPoints(leftPoint);
                //noteB.CheckIntersectionWithPoints(rightPoint);
            }
        }

        Skeleton GetFirstSkeleton(AllFramesReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrameData = e.OpenSkeletonFrame())
            {
                if (skeletonFrameData == null)
                {
                    return null;
                }


                skeletonFrameData.CopySkeletonDataTo(allSkeletons);

                //get the first tracked skeleton
                Skeleton first = (from s in allSkeletons
                                  where s.TrackingState == SkeletonTrackingState.Tracked
                                  select s).FirstOrDefault();

                return first;

            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            closing = true;
            StopKinect(kinectSensorChooser1.Kinect); 
        }



        
    }
}
