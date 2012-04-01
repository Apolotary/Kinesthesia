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
using Microsoft.Win32;
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
        MidiManager midMan = MidiManager.Instance;
        Skeleton[] allSkeletons = new Skeleton[skeletonCount];
        private GestureRecognizer leftHandRecognizer;
        private GestureRecognizer rightHandRecognizer;
        private List<TrackPlayer> trackPlayers;
        private bool shouldTrack = false;
        private string[] notes = {"C", "D", "E", "F", "G", "A", "B"};
        private int currNote = 0;
        private int currOctave = 5;
        private int currVelocity = 80;
        private TrackPlayer track1;
        private TrackPlayer track2;
        private TrackPlayer track3;
        private bool isPlaying = false;
        MidiPlayer midiPl = new MidiPlayer();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);

            leftHandRecognizer = new GestureRecognizer();
            rightHandRecognizer = new GestureRecognizer();
            rightHandRecognizer.FramesToCompare = 60;
            leftHandRecognizer.FramesToCompare = 60;
            rightHandRecognizer.Threshold = 30;
            leftHandRecognizer.Threshold = 30;

            leftHandRecognizer.XaxisIncreased += new EventHandler(Empty);
            leftHandRecognizer.XaxisDecreased += new EventHandler(Empty);
            leftHandRecognizer.YaxisIncreased += new EventHandler(ChangeVelocity);
            leftHandRecognizer.YaxisDecreased += new EventHandler(ChangeVelocity);

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            Type gestClass = typeof(GestureRecognizer);
            EventInfo xAxisEvent = gestClass.GetEvent("XaxisIncreased", bindingFlags);

            Delegate handler = Delegate.CreateDelegate(xAxisEvent.EventHandlerType, this, "Empty");
            xAxisEvent.AddEventHandler(rightHandRecognizer, handler);

            //rightHandRecognizer.XaxisIncreased += new EventHandler(SendNote);
            rightHandRecognizer.XaxisDecreased += new EventHandler(Empty);
            rightHandRecognizer.YaxisIncreased += new EventHandler(ChangeVelocity);
            rightHandRecognizer.YaxisDecreased += new EventHandler(ChangeVelocity);
            //ParseConfigs();
        }
        
        private void ParseConfigs()
        {
            //logBlock.Text = Convert.ToString(trList[2].Notes[5].Note);
        }

        private void ParseCSVAtPath(string path)
        {
            List<Track> trList = midiPl.ParseMIDIFileInCSV(path);

            trackPlayers = new List<TrackPlayer>();

            track1 = new TrackPlayer(trList[0]);
            track2 = new TrackPlayer(trList[2]);
            track3 = new TrackPlayer(trList[1]);
        }

        private void ChangeVelocity(object sender, EventArgs e)
        {
            GestureEventArgs ge = (GestureEventArgs)e;

            int velocity = ValueToVelocity(480, ge.point.Y);
            if (velocity > 127) velocity = 127;

            if(ge.joint.JointType == JointType.HandLeft)
            {
                track2.Velocity = velocity;
                logBlock.Text += "\n TRACK " + track2.Track.TrackNumber + " VELOCITY " + velocity + " X: " + ge.point.X + " Y: " + ge.point.Y;
                ScrollTheBox();
            }
            else
            {
                track3.Velocity = velocity;
                logBlock.Text += "\n TRACK " + track3.Track.TrackNumber + " VELOCITY " + velocity + " X: " + ge.point.X + " Y: " + ge.point.Y;
                ScrollTheBox();
            }
        }


        private int ValueToVelocity (int scope, float value)
        {
            return Convert.ToInt32(127*value/scope);
        }
        
        private void Empty (object sender, EventArgs e)
        {
            
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

                leftHandRecognizer.TrackedJoint = left;
                rightHandRecognizer.TrackedJoint = right;

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

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (!isPlaying)
            {
                playButton.Content = "Pause";
                isPlaying = true;
                midiPl.PlayParsedFile();
            }
            else
            {
                playButton.Content = "Play";
                isPlaying = false;
                midMan.Clock.Stop();
            }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            string input = string.Empty;
 
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
 
            dialog.InitialDirectory = "C:"; 
            dialog.Title = "Select a CSV file";

            dialog.ShowDialog();

            if (dialog.FileName != string.Empty)
            {
                logBlock.Text += "OPEN CSV FILE AT: " + dialog.FileName;
                ScrollTheBox();
                ParseCSVAtPath(dialog.FileName);
            }
              
        }
    }
}
