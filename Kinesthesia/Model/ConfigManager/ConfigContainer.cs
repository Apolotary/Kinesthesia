using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace Kinesthesia.Model.ConfigManager
{
    /// <summary>
    /// Container for the current configuration
    /// </summary>
    class ConfigContainer
    {
        /// <summary>
        /// name of the joint which triggers the event
        /// </summary>
        private JointType _jointType;

        /// <summary>
        /// minimal value for length of swipe to trigger gesture
        /// </summary>
        private double _swipeMinimalLength;

        /// <summary>
        /// maximal value for length of swipe to trigger gesture
        /// </summary>
        private double _swipeMaximalLength;

        /// <summary>
        /// minimal value for heigth of swipe to trigger gesture
        /// </summary>
        private double _swipeMinimalHeight;

        /// <summary>
        /// maximal value for length of swipe to trigger gesture
        /// </summary>
        private double _swipeMaximalHeight;

        /// <summary>
        /// minimal value for duration of swipe in miliseconds to trigger gesture
        /// </summary>
        private int _swipeMinimalDuration;

        /// <summary>
        /// maximal value for duration of swipe in miliseconds to trigger gesture
        /// </summary>
        private int _swipeMaximalDuration;
        
        /// <summary>
        /// event name which triggers the method
        /// </summary>
        private string _eventName;

        /// <summary>
        /// method name that will be triggered
        /// </summary>
        private string _methodName;

        /// <summary>
        /// type of config container
        /// it can be "Calibration" -- for calibrating gesture sensitivity
        /// or "Event" -- for binding methods and events
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
        }

        /// <summary>
        /// constructor with params
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
        public ConfigContainer(JointType joint,
                               double swipeMinimalLength,
                               double swipeMaximalLength,
                               double swipeMinimalHeight,
                               double swipeMaximalHeight,
                               int swipeMinimalDuration,
                               int swipeMaximalDuration,
                               string eventName, 
                               string methodName,
                               string configType)
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
        }
    }
}
