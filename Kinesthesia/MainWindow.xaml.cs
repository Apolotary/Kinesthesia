using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Kinesthesia.Model.GestureRecognition;
using Coding4Fun.Kinect.Wpf;
using System.Reflection;
using Kinesthesia.Model.ConfigManager;

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
        private GestureRecognizer leftHandRecognizer;
        private GestureRecognizer rightHandRecognizer;
        private bool shouldTrack = false;
        private string[] notes = {"C", "D", "E", "F", "G", "A", "B"};
        private int currNote = 0;
        private int currOctave = 5;
        private int currVelocity = 80;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);

            leftHandRecognizer = new GestureRecognizer();
            rightHandRecognizer = new GestureRecognizer();
            rightHandRecognizer.FramesToCompare = 15;

            leftHandRecognizer.XaxisIncreased += new EventHandler(ChangeVolume);
            leftHandRecognizer.XaxisDecreased += new EventHandler(ChangeVolume);
            leftHandRecognizer.YaxisIncreased += new EventHandler(BendPitch);
            leftHandRecognizer.YaxisDecreased += new EventHandler(BendPitch);

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            Type gestClass = typeof (GestureRecognizer);
            EventInfo xAxisEvent = gestClass.GetEvent("XaxisIncreased", bindingFlags);

            Delegate handler = Delegate.CreateDelegate(xAxisEvent.EventHandlerType, this, "SendNote");
            xAxisEvent.AddEventHandler(rightHandRecognizer, handler);

            //rightHandRecognizer.XaxisIncreased += new EventHandler(SendNote);
            rightHandRecognizer.XaxisDecreased += new EventHandler(SendNote);
            rightHandRecognizer.YaxisIncreased += new EventHandler(SendNote);
            rightHandRecognizer.YaxisDecreased += new EventHandler(SendNote);
        }
        
        private void ParseConfigs()
        {
            ConfigParser cParser = new ConfigParser();
            List<ConfigContainer> cList = cParser.ParseSettingsFileTXT("default.txt");
            logBlock.Text = cList[0].JointName;
        }

        private void SendNote (object sender, EventArgs e)
        {
            GestureEventArgs ge = (GestureEventArgs)e;

            int octave = 640/7;
            int note = octave/7;
            int velocity = 480/127;

            midMan.SendNoteOffMessage(notes[currNote], currOctave, currVelocity);

            currOctave = (int) ge.point.X/octave;
            currNote = (int) ge.point.X/note - currOctave*7;
            currVelocity = (int) ge.point.Y/velocity;

            if (currOctave > 7) currOctave = 7;
            if (currNote > 7) currNote = 7;
            if (currVelocity > 127) currVelocity = 127;

            midMan.SendNoteOnMessage(notes[currNote], currOctave, currVelocity);

            logBlock.Text += "\n NOTE " + notes[currNote] + " OF OCTAVE " + currOctave + " VELOCITY " + currVelocity + " X: " + ge.point.X + " Y: " + ge.point.Y;
            ScrollTheBox();
        }

        private void BendPitch (object sender, EventArgs e)
        {
            GestureEventArgs ge = (GestureEventArgs) e;
            
            int pitchPart = 16384/480;
            midMan.SendPitchBend(pitchPart*(int)ge.point.Y);

            logBlock.Text += "\n PITCH BEND " + pitchPart * (int)ge.point.Y + " " + leftHandRecognizer.currPointNumber() + " X: " + ge.point.X + " Y: " + ge.point.Y;
            ScrollTheBox();
        }

        private void ChangeVolume (object sender, EventArgs e)
        {
            GestureEventArgs ge = (GestureEventArgs)e;
            
            int volumePart = 640/127;
            int vol = (int)ge.point.X/volumePart;
            

            logBlock.Text += "\n CHANGE VOLUME " + vol + " " + leftHandRecognizer.currPointNumber() + " X: " + ge.point.X + " Y: " + ge.point.Y;
            ScrollTheBox();
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

            Joint left = first.Joints[JointType.HandLeft];
            Joint right = first.Joints[JointType.HandRight];
            
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

                Point[] points = { leftPoint, rightPoint };

                label2.Content = leftPoint.X;
                label3.Content = leftPoint.Y;
                label5.Content = rightPoint.X;
                label6.Content = rightPoint.Y;

                if (shouldTrack)
                {
                    SkeletonPoint lpoint = new SkeletonPoint();
                    lpoint.X = (float) leftPoint.X;
                    lpoint.Y = (float) leftPoint.Y;
                    lpoint.Z = 0;

                    SkeletonPoint rpoint = new SkeletonPoint();
                    rpoint.X = (float)rightPoint.X;
                    rpoint.Y = (float)rightPoint.Y;
                    rpoint.Z = 0;

                    leftHandRecognizer.AddCoordinate(lpoint);
                    rightHandRecognizer.AddCoordinate(rpoint);

                    //logBlock.Text += "\n NEW POINT " + leftHandRecognizer.currPointNumber() + " X: " + point.X + " Y: " + point.Y;
                }
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

        private void trackButton_Click(object sender, RoutedEventArgs e)
        {
            if (!shouldTrack)
            {
                trackButton.Content = "Stop";
                shouldTrack = true;
                logBlock.Text = "Start Tracking";
                ScrollTheBox();
            }
            else
            {
                midMan.SendPitchBend(8192);
                midMan.SendNoteOffMessage(notes[currNote], currOctave, currVelocity);
                trackButton.Content = "Start";
                shouldTrack = false;
                logBlock.Text += "\n Stop Tracking";
                ScrollTheBox();
            }
        }
        
        private void ScrollTheBox()
        {
            logBlock.SelectionStart = logBlock.Text.Length;
            logBlock.ScrollToEnd();
        }
    }
}
