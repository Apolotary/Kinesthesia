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
using Kinect.Toolbox;

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

        private List<ConfigContainer> configurationList;

        /// <summary>
        /// gesture recognizers
        /// </summary>
        private SwipeGestureDetector rightHandSwipeDetector;
        private SwipeGestureDetector leftHandSwipeDetector;

        private List<TrackPlayer> trackPlayers;
        private bool shouldTrack = false;

        private string[] notes = {"C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B"};
        private int currNote = 0;
        private int currOctave = 5;
        private int currVelocity = 80;

        private List<Track> trackList;
        private TrackPlayer track1;
        private TrackPlayer track2;
        private TrackPlayer track3;
        private bool isPlaying = false;
        MidiPlayer midiPl = new MidiPlayer();

        private KinectSensor sensor;
        private Point lastPoint;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            kinectSensorChooser1.KinectSensorChanged += new DependencyPropertyChangedEventHandler(kinectSensorChooser1_KinectSensorChanged);
            CreateAndInitializeDefaultGestureRecognizers();
        }

        private void CreateAndInitializeDefaultGestureRecognizers()
        {
            rightHandSwipeDetector = new SwipeGestureDetector();
            rightHandSwipeDetector.OnGestureDetected += OnRightGestureDetected;

            leftHandSwipeDetector = new SwipeGestureDetector();
            leftHandSwipeDetector.OnGestureDetected += OnLeftGestureDetected;

            ParseConfigs(@"c:\diploma\Kinesthesia\Kinesthesia\SupportingFiles\default.csv");
        }
        
        void OnRightGestureDetected(string gesture, Point p)
        {
            logBlock.Text += "\n" + "RIGHT_HAND" + gesture  +
                             " X: " + lastPoint.X + " Y: " + lastPoint.Y;
            ScrollTheBox();

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            string methodName = "DoNothing";
            foreach (var configContainer in configurationList)
            {
                if (configContainer.JointType == JointType.HandRight && configContainer.EventName == gesture)
                {
                    methodName = configContainer.MethodName;
                }
            }

            object[] args = new object[2];
            args[0] = JointType.HandRight;
            args[1] = lastPoint;
            MethodInfo method = typeof (MainWindow).GetMethod(methodName, bindingFlags);
            method.Invoke(this, args);
        }

        void OnLeftGestureDetected(string gesture, Point p)
        {
            logBlock.Text += "\n" + "LEFT_HAND" + gesture +
                             " X: " + lastPoint.X + " Y: " + lastPoint.Y;
            ScrollTheBox();

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            string methodName = "DoNothing";
            foreach (var configContainer in configurationList)
            {
                if (configContainer.JointType == JointType.HandLeft && configContainer.EventName == gesture)
                {
                    methodName = configContainer.MethodName;
                }
            }

            object[] args = new object[2];
            args[0] = JointType.HandLeft;
            args[1] = lastPoint;
            MethodInfo method = typeof(MainWindow).GetMethod(methodName, bindingFlags);
            method.Invoke(this, args);
        }

        private void ParseConfigs(string path)
        {
            ConfigParser configParser = new ConfigParser();
            configurationList = configParser.ParseConfigs(path);

            settingsLog.Text = "";
            
            foreach (var configContainer in configurationList)
            {
                if (configContainer.ConfigType == "Calibration")
                {
                    settingsLog.Text += "\n" + Convert.ToString(configContainer.JointType) + "\n         " +
                                        "min length = " + configContainer.SwipeMinimalLength + " " +
                                        "max length = " + configContainer.SwipeMaximalLength + "\n         " +
                                        "min height = " + configContainer.SwipeMinimalHeight + " " +
                                        "max height = " + configContainer.SwipeMaximalHeight + "\n         " +
                                        "min duration = " + configContainer.SwipeMinimalDuration + " " +
                                        "max duration = " + configContainer.SwipeMaximalDuration + "\n";
                    ScrollTheBox();

                    if (configContainer.JointType == JointType.HandRight)
                    {
                        rightHandSwipeDetector.SwipeMinimalLength = configContainer.SwipeMinimalLength;
                        rightHandSwipeDetector.SwipeMaximalLength = configContainer.SwipeMaximalLength;
                        rightHandSwipeDetector.SwipeMinimalHeight = configContainer.SwipeMinimalHeight;
                        rightHandSwipeDetector.SwipeMaximalHeight = configContainer.SwipeMaximalHeight;
                        rightHandSwipeDetector.SwipeMinimalDuration = configContainer.SwipeMinimalDuration;
                        rightHandSwipeDetector.SwipeMaximalDuration = configContainer.SwipeMaximalDuration;

                        rHandMinLengthSlider.Value = configContainer.SwipeMinimalLength;
                        rHandMaxLengthSlider.Value = configContainer.SwipeMaximalLength;
                        rHandMinHeightSlider.Value = configContainer.SwipeMinimalHeight;
                        rHandMaxHeightSlider.Value = configContainer.SwipeMaximalHeight;
                        rHandMinDurationSlider.Value = configContainer.SwipeMinimalDuration;
                        rHandMaxDurationSlider.Value = configContainer.SwipeMaximalDuration;

                        rHandMinLengthSliderBox.Content = configContainer.SwipeMinimalLength;
                        rHandMaxLengthSliderBox.Content = configContainer.SwipeMaximalLength;
                        rHandMinHeightSliderBox.Content = configContainer.SwipeMinimalHeight;
                        rHandMaxHeightSliderBox.Content = configContainer.SwipeMaximalHeight;
                        rHandMinDurationSliderBox.Content = configContainer.SwipeMinimalDuration;
                        rHandMaxDurationSliderBox.Content = configContainer.SwipeMaximalDuration;
                    }
                    else if (configContainer.JointType == JointType.HandLeft)
                    {
                        leftHandSwipeDetector.SwipeMinimalLength = configContainer.SwipeMinimalLength;
                        leftHandSwipeDetector.SwipeMaximalLength = configContainer.SwipeMaximalLength;
                        leftHandSwipeDetector.SwipeMinimalHeight = configContainer.SwipeMinimalHeight;
                        leftHandSwipeDetector.SwipeMaximalHeight = configContainer.SwipeMaximalHeight;
                        leftHandSwipeDetector.SwipeMinimalDuration = configContainer.SwipeMinimalDuration;
                        leftHandSwipeDetector.SwipeMaximalDuration = configContainer.SwipeMaximalDuration;

                        lHandMinLengthSlider.Value = configContainer.SwipeMinimalLength;
                        lHandMaxLengthSlider.Value = configContainer.SwipeMaximalLength;
                        lHandMinHeightSlider.Value = configContainer.SwipeMinimalHeight;
                        lHandMaxHeightSlider.Value = configContainer.SwipeMaximalHeight;
                        lHandMinDurationSlider.Value = configContainer.SwipeMinimalDuration;
                        lHandMaxDurationSlider.Value = configContainer.SwipeMaximalDuration;

                        lHandMinLengthSliderBox.Content = configContainer.SwipeMinimalLength;
                        lHandMaxLengthSliderBox.Content = configContainer.SwipeMaximalLength;
                        lHandMinHeightSliderBox.Content = configContainer.SwipeMinimalHeight;
                        lHandMaxHeightSliderBox.Content = configContainer.SwipeMaximalHeight;
                        lHandMinDurationSliderBox.Content = configContainer.SwipeMinimalDuration;
                        lHandMaxDurationSliderBox.Content = configContainer.SwipeMaximalDuration;
                    }
                }
                else if (configContainer.ConfigType == "Event")
                {
                    settingsLog.Text += "\n" + Convert.ToString(configContainer.JointType) + " " +
                                        configContainer.EventName + " " + configContainer.MethodName;
                    ScrollTheBox();
                }
            }
        }

        private void ParseCSVAtPath(string path)
        {
            trackList = midiPl.ParseMIDIFileInCSV(path);

            PrintTrackListToSettingsLog();

            trackPlayers = new List<TrackPlayer>();

            track1 = new TrackPlayer(trackList[0]);
            track2 = new TrackPlayer(trackList[2]);
            track3 = new TrackPlayer(trackList[1]);
        }

        private void PrintTrackListToSettingsLog()
        {
            settingsLog.Text = "MIDI Track Properties: \n \n";
            foreach (var track in trackList)
            {
                settingsLog.Text += track.TrackNumber + " " + track.TrackName + "\n";
            }
        }

        private void ChangeVelocity(JointType joint, Point point)
        {
            int velocity = ValueToVelocity(480, point.Y);
            if (velocity > 127) velocity = 127;

            if(joint == JointType.HandLeft)
            {
                track2.Velocity = velocity;
                logBlock.Text += "\nTRACK " + track2.Track.TrackNumber + " VELOCITY " + velocity + " X: " + point.X + " Y: " + point.Y;
                ScrollTheBox();
            }
            else
            {
                track3.Velocity = velocity;
                logBlock.Text += "\nTRACK " + track3.Track.TrackNumber + " VELOCITY " + velocity + " X: " + point.X + " Y: " + point.Y;
                ScrollTheBox();
            }
        }


        private int ValueToVelocity (int scope, double value)
        {
            return Convert.ToInt32(127*value/scope);
        }

        private void DoNothing(JointType joint, Point point)
        {
            
        }

        private void SendNote(JointType joint, Point point)
        {
            int octave = 640/7;
            int note = octave/7;
            int velocity = 480/127;

            midMan.SendNoteOffMessage(notes[currNote], currOctave, currVelocity);

            currOctave = (int) point.X/octave;
            currNote = (int) point.X/note - currOctave*7;
            currVelocity = (int) point.Y/velocity;

            if (currOctave > 7) currOctave = 7;
            if (currNote > 7) currNote = 7;
            if (currVelocity > 127) currVelocity = 127;

            midMan.SendNoteOnMessage(notes[currNote], currOctave, currVelocity);

            logBlock.Text += "\nNOTE " + notes[currNote] + " OF OCTAVE " + currOctave + " VELOCITY " + currVelocity + " X: " + point.X + " Y: " + point.Y;
            ScrollTheBox();
        }

        private void BendPitch(JointType joint, Point point)
        {
            int pitchPart = 16384/480;
            midMan.SendPitchBend(pitchPart*(int)point.Y);

            logBlock.Text += "\nPITCH BEND " + pitchPart * (int)point.Y + " X: " + point.X + " Y: " + point.Y;
            ScrollTheBox();
        }

        private void ChangeVolume(JointType joint, Point point)
        {
            int volumePart = 640/127;
            int vol = (int)point.X/volumePart;
            
            logBlock.Text += "\nCHANGE VOLUME " + vol + " X: " + point.X + " Y: " + point.Y;
            ScrollTheBox();
        }


        private void kinectSensorChooser1_KinectSensorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            KinectSensor old = (KinectSensor)e.OldValue;

            StopKinect(old);

            sensor = (KinectSensor)e.NewValue;

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

                    //leftHandRecognizer.AddCoordinate(lpoint);
                    //rightHandRecognizer.AddCoordinate(rpoint);
                    lastPoint = rightPoint;
                    //swipeDetector.Add(right.Position, sensor);
                    rightHandSwipeDetector.Add(right.Position, sensor);
                    leftHandSwipeDetector.Add(left.Position, sensor);

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
                lHandMinLengthSlider.IsEnabled = false;
                lHandMaxLengthSlider.IsEnabled = false;
                lHandMinHeightSlider.IsEnabled = false;
                lHandMaxHeightSlider.IsEnabled = false;
                lHandMinDurationSlider.IsEnabled = false;
                lHandMaxDurationSlider.IsEnabled = false;

                rHandMinLengthSlider.IsEnabled = false;
                rHandMaxLengthSlider.IsEnabled = false;
                rHandMinHeightSlider.IsEnabled = false;
                rHandMaxHeightSlider.IsEnabled = false;
                rHandMinDurationSlider.IsEnabled = false;
                rHandMaxDurationSlider.IsEnabled = false;

                trackButton.Content = "Stop";
                shouldTrack = true;
                logBlock.Text += "\nStart Tracking";
                ScrollTheBox();
            }
            else
            {
                lHandMinLengthSlider.IsEnabled = true;
                lHandMaxLengthSlider.IsEnabled = true;
                lHandMinHeightSlider.IsEnabled = true;
                lHandMaxHeightSlider.IsEnabled = true;
                lHandMinDurationSlider.IsEnabled = true;
                lHandMaxDurationSlider.IsEnabled = true;

                rHandMinLengthSlider.IsEnabled = true;
                rHandMaxLengthSlider.IsEnabled = true;
                rHandMinHeightSlider.IsEnabled = true;
                rHandMaxHeightSlider.IsEnabled = true;
                rHandMinDurationSlider.IsEnabled = true;
                rHandMaxDurationSlider.IsEnabled = true;


                midMan.SendPitchBend(8192);
                midMan.SendNoteOffMessage(notes[currNote], currOctave, currVelocity);
                trackButton.Content = "Start";
                shouldTrack = false;
                logBlock.Text += "\nStop Tracking";
                ScrollTheBox();
            }
        }
        
        private void ScrollTheBox()
        {
            logBlock.SelectionStart = logBlock.Text.Length;
            logBlock.ScrollToEnd();

            settingsLog.SelectionStart = settingsLog.Text.Length;
            settingsLog.ScrollToEnd();
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

            dialog.InitialDirectory = Environment.CurrentDirectory; 
            dialog.Title = "Select a CSV file";

            dialog.ShowDialog();

            if (dialog.FileName != string.Empty)
            {
                logBlock.Text += "OPEN CSV FILE AT: " + dialog.FileName;
                ScrollTheBox();
                ParseCSVAtPath(dialog.FileName);
            }
              
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            string input = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";

            dialog.InitialDirectory = Environment.CurrentDirectory;
            dialog.Title = "Select a CSV file";

            dialog.ShowDialog();

            if (dialog.FileName != string.Empty)
            {
                logBlock.Text += "OPEN CONFIG FILE AT: " + dialog.FileName;
                ScrollTheBox();
                ParseConfigs(dialog.FileName);
            }
        }

        private void lHandMinLengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftHandSwipeDetector.SwipeMinimalLength = e.NewValue;
            lHandMinLengthSliderBox.Content = string.Format("{0:0.00}", e.NewValue);
        }

        private void lHandMaxLengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftHandSwipeDetector.SwipeMaximalLength = e.NewValue;
            lHandMaxLengthSliderBox.Content = string.Format("{0:0.00}", e.NewValue);
        }

        private void lHandMinHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftHandSwipeDetector.SwipeMinimalHeight = e.NewValue;
            lHandMinHeightSliderBox.Content = string.Format("{0:0.00}", e.NewValue);
        }

        private void lHandMaxHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftHandSwipeDetector.SwipeMaximalHeight = e.NewValue;
            lHandMaxHeightSliderBox.Content = string.Format("{0:0.00}", e.NewValue);
        }

        private void lHandMinDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftHandSwipeDetector.SwipeMinimalDuration = Convert.ToInt32(e.NewValue);
            lHandMinDurationSliderBox.Content = Convert.ToInt32(e.NewValue);
        }

        private void lHandMaxDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            leftHandSwipeDetector.SwipeMaximalDuration = Convert.ToInt32(e.NewValue);
            lHandMaxDurationSliderBox.Content = Convert.ToInt32(e.NewValue);
        }

        private void rHandMinLengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rightHandSwipeDetector.SwipeMinimalLength = e.NewValue;
            rHandMinLengthSliderBox.Content = string.Format("{0:0.00}", e.NewValue);
        }

        private void rHandMaxLengthSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rightHandSwipeDetector.SwipeMaximalLength = e.NewValue;
            rHandMaxLengthSliderBox.Content = string.Format("{0:0.00}", e.NewValue);
        }

        private void rHandMinHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rightHandSwipeDetector.SwipeMinimalHeight = e.NewValue;
            rHandMinHeightSliderBox.Content = string.Format("{0:0.00}", e.NewValue);
        }

        private void rHandMaxHeightSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rightHandSwipeDetector.SwipeMaximalHeight = e.NewValue;
            rHandMaxHeightSliderBox.Content = string.Format("{0:0.00}", e.NewValue);
        }

        private void rHandMinDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rightHandSwipeDetector.SwipeMinimalDuration = Convert.ToInt32(e.NewValue);
            rHandMinDurationSliderBox.Content = Convert.ToInt32(e.NewValue);
        }

        private void rHandMaxDurationSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            rightHandSwipeDetector.SwipeMaximalDuration = Convert.ToInt32(e.NewValue);
            rHandMaxDurationSliderBox.Content = Convert.ToInt32(e.NewValue);
        }
    }
}
