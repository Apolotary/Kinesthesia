using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Kinesthesia.Model.ConfigManager
{
    /// <summary>
    /// Container for the current configuration of application
    /// </summary>
    class ConfigContainer
    {
        /// <summary>
        /// Name of the joint which triggers the event
        /// </summary>
        private JointType _jointType;

        /// <summary>
        /// Minimal value for length of swipe to trigger gesture
        /// </summary>
        private double _swipeMinimalLength;

        /// <summary>
        /// Maximal value for length of swipe to trigger gesture
        /// </summary>
        private double _swipeMaximalLength;

        /// <summary>
        /// Minimal value for heigth of swipe to trigger gesture
        /// </summary>
        private double _swipeMinimalHeight;

        /// <summary>
        /// Maximal value for length of swipe to trigger gesture
        /// </summary>
        private double _swipeMaximalHeight;

        /// <summary>
        /// Minimal value for duration of swipe in miliseconds to trigger gesture
        /// </summary>
        private int _swipeMinimalDuration;

        /// <summary>
        /// Maximal value for duration of swipe in miliseconds to trigger gesture
        /// </summary>
        private int _swipeMaximalDuration;
        
        /// <summary>
        /// Rvent name which triggers the method
        /// </summary>
        private string _eventName;

        /// <summary>
        /// Method name that will be triggered
        /// </summary>
        private string _methodName;

        /// <summary>
        /// List with notes for scale which will be used to pick notes
        /// </summary>
        private List<string> _scale;
        
        /// <summary>
        /// Quantity of notes that will be used for MIDI-messages
        /// MIDI supports up to 127 notes
        /// </summary>
        private int _quantityOfNotes;

        /// <summary>
        /// Determines the orientation for MIDI notes on screen
        /// </summary>
        private bool _isHorizontal;

        /// <summary>
        /// Keyword for voice command
        /// </summary>
        private string _keyWord;

        /// <summary>
        /// Method for voice command
        /// </summary>
        private string _voiceCommand;

        /// <summary>
        /// Minimal velocity value, from 0 to 127
        /// </summary>
        private int _minVelocity;

        /// <summary>
        /// Maximal velocity value, from 0 to 127
        /// </summary>
        private int _maxVelocity;
        
        /// <summary>
        /// Type of config container:
        /// "Calibration" -- for calibrating gesture sensitivity
        /// "Scale" -- for array of notes which will be used within one scale
        /// "Note" -- for quantity of notes and their orientation
        /// "Velocity" -- minimal and maximal velocity values for ChangeVelocity methods
        /// "Event" -- for binding methods and events
        /// "Voice" -- keyword and method for voice command control
        /// </summary>
        private string _configType;

        /// <summary>
        /// properties
        /// </summary>
        public JointType JointType
        {
            get { return _jointType; }
            set { _jointType = value; }
        }
        public double SwipeMinimalLength
        {
            get { return _swipeMinimalLength; }
            set { _swipeMinimalLength = value; }
        }
        public double SwipeMaximalLength
        {
            get { return _swipeMaximalLength; }
            set { _swipeMaximalLength = value; }
        }
        public double SwipeMinimalHeight
        {
            get { return _swipeMinimalHeight; }
            set { _swipeMinimalHeight = value; }
        }
        public double SwipeMaximalHeight
        {
            get { return _swipeMaximalHeight; }
            set { _swipeMaximalHeight = value; }
        }
        public int SwipeMinimalDuration
        {
            get { return _swipeMinimalDuration; }
            set { _swipeMinimalDuration = value; }
        }
        public int SwipeMaximalDuration
        {
            get { return _swipeMaximalDuration; }
            set { _swipeMaximalDuration = value; }
        }
        public string EventName
        {
            get { return _eventName; }
            set { _eventName = value; }
        }
        public string MethodName
        {
            get { return _methodName; }
            set { _methodName = value; }
        }
        public List<string> Scale
        {
            get { return _scale; }
            set { _scale = value; }
        }
        public int QuantityOfNotes
        {
            get { return _quantityOfNotes; }
            set { _quantityOfNotes = value; }
        }
        public bool IsHorizontal
        {
            get { return _isHorizontal; }
            set { _isHorizontal = value; }
        }
        public string KeyWord
        {
            get { return _keyWord; }
            set { _keyWord = value; }
        }
        public string VoiceCommand
        {
            get { return _voiceCommand; }
            set { _voiceCommand = value; }
        }
        public int MinVelocity
        {
            get { return _minVelocity; }
            set { _minVelocity = value; }
        }
        public int MaxVelocity
        {
            get { return _maxVelocity; }
            set { _maxVelocity = value; }
        }
        public string ConfigType
        {
            get { return _configType; }
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public ConfigContainer()
        {
            _jointType = JointType.Head;
            _swipeMinimalLength = 0.4;
            _swipeMaximalLength = 0.2;
            _swipeMinimalHeight = 0.1;
            _swipeMaximalHeight = 0.2;
            _swipeMinimalDuration = 250;
            _swipeMaximalDuration = 1500;
            _eventName = "";
            _methodName = "";
            _configType = "Calibration";
            _scale = new List<string>();
            _quantityOfNotes = 0;
            _isHorizontal = true;
            _keyWord = "";
            _voiceCommand = "";
            _minVelocity = 0;
            _maxVelocity = 127;
        }


        /// <summary>
        /// constructor with parameters
        /// </summary>
        /// <param name="joint">type of the joint which triggers the event</param>
        /// <param name="swipeMinimalLength">minimal value for length of swipe to trigger gesture</param>
        /// <param name="swipeMaximalLength">maximal value for length of swipe to trigger gesture</param>
        /// <param name="swipeMinimalHeight">minimal value for heigth of swipe to trigger gesture</param>
        /// <param name="swipeMaximalHeight">maximal value for length of swipe to trigger gesture</param>
        /// <param name="swipeMinimalDuration">minimal value for duration of swipe in miliseconds to trigger gesture</param>
        /// <param name="swipeMaximalDuration">maximal value for duration of swipe in miliseconds to trigger gesture</param>
        /// <param name="eventName">event name which triggers the method</param>
        /// <param name="methodName">method name that will be triggered</param>
        /// <param name="configType">type of config container</param>
        /// <param name="scale">list with notes for scale which will be used to pick notes</param>
        /// <param name="quantityOfNotes">quantity of notes that will be used for picking
        /// MIDI supports up to 127 notes</param>
        /// <param name="isHorizontal">determines the orientation for MIDI notes on screen</param>
        /// <param name="keyWord">keyword for voice command</param>
        /// <param name="voiceCommand">method for voice command</param>
        /// <param name="minVelocity">minimal velocity value</param>
        /// <param name="maxVelocity">maximal velocity value</param>
        public ConfigContainer(JointType joint,
                               double swipeMinimalLength,
                               double swipeMaximalLength,
                               double swipeMinimalHeight,
                               double swipeMaximalHeight,
                               int swipeMinimalDuration,
                               int swipeMaximalDuration,
                               string eventName, 
                               string methodName,
                               string configType,
                               List<string> scale,
                               int quantityOfNotes, 
                               bool isHorizontal, 
                               string keyWord,
                               string voiceCommand,
                               int minVelocity,
                               int maxVelocity)
        {
            _jointType = joint;
            _swipeMinimalLength = swipeMinimalLength;
            _swipeMaximalLength = swipeMaximalLength;
            _swipeMinimalHeight = swipeMinimalHeight;
            _swipeMaximalHeight = swipeMaximalHeight;
            _swipeMinimalDuration = swipeMinimalDuration;
            _swipeMaximalDuration = swipeMaximalDuration;
            _eventName = eventName;
            _methodName = methodName;
            _configType = configType;
            _scale = scale;
            _quantityOfNotes = quantityOfNotes;
            _isHorizontal = isHorizontal;
            _keyWord = keyWord;
            _voiceCommand = voiceCommand;
            _minVelocity = minVelocity;
            _maxVelocity = maxVelocity;
        }
    }
}
