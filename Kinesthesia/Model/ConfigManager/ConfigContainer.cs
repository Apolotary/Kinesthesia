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
        /// threshold value to trigger
        /// </summary>
        private int    _threshold;
        
        /// <summary>
        /// event name which triggers the method
        /// </summary>
        private string _eventName;

        /// <summary>
        /// method name that will be triggered
        /// </summary>
        private string _methodName;

        /// <summary>
        /// properties
        /// </summary>
        public JointType JointType
        {
            get { return _jointType; }
            set { _jointType = value; }
        }
        public int Threshold
        {
            get { return _threshold; }
            set { _threshold = value; }
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

        /// <summary>
        /// default constructor
        /// </summary>
        public ConfigContainer()
        {
            _jointType = JointType.Head;
            _threshold = 10;
            _eventName = "";
            _methodName = "";
        }

        /// <summary>
        /// constructor with params
        /// </summary>
        /// <param name="joint">type of the joint which triggers the event</param>
        /// <param name="threshold">threshold value to trigger</param>
        /// <param name="eventName">event name which triggers the method</param>
        /// <param name="methodName">method name that will be triggered</param>
        public ConfigContainer(JointType joint, int threshold, string eventName, string methodName)
        {
            _jointType = joint;
            _threshold = threshold;
            _eventName = eventName;
            _methodName = methodName;
        }
    }
}
