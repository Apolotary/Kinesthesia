﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinesthesia.Model.ConfigManager
{
    class ConfigContainer
    {
        /// <summary>
        /// name of the joint which triggers the event
        /// </summary>
        private string _jointName;

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
        public string JointName { get; set; }
        public int Threshold { get; set; }
        public string EventName { get; set; }
        public string MethodName { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public ConfigContainer()
        {
            _jointName = "";
            _threshold = 10;
            _eventName = "";
            _methodName = "";
        }

        /// <summary>
        /// constructor with params
        /// </summary>
        /// <param name="j">name of the joint which triggers the event</param>
        /// <param name="t">threshold value to trigger</param>
        /// <param name="e">event name which triggers the method</param>
        /// <param name="m">method name that will be triggered</param>
        public ConfigContainer(string j, int t, string e, string m)
        {
            _jointName = j;
            _threshold = t;
            _eventName = e;
            _methodName = m;
        }
    }
}