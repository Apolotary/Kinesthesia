using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Kinect;
using System.Text;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace Kinesthesia.Model.ConfigManager
{
    /// <summary>
    /// Class that parses the config CSV files into configuration containers
    /// </summary>
    class ConfigParser
    {
        private List<ConfigContainer> configList;
        
        /// <summary>
        /// Parsing configuration CSV file at given path
        /// into configuration list which will be later used within the application.
        /// </summary>
        /// <param name="path"></param>
        public List<ConfigContainer> ParseConfigs(string path)
        {
            List<string[]> parsedData = CSVParser.parseCSV(path);

            configList = new List<ConfigContainer>();

            for (int i = 0; i < parsedData.Count(); ++i )
            {
                string joint = "";
                double swipeMinimalLength = 0.0;
                double swipeMaximalLength = 0.0;
                double swipeMinimalHeight = 0.0;
                double swipeMaximalHeight = 0.0;
                int swipeMinimalDuration = 0;
                int swipeMaximalDuration = 0;
                string eventName = "";
                string methodName = "";
                string configType = "";
                List<string> scale = new List<string>();
                int quantityOfNotes = 0;
                configType = parsedData[i][0];
                bool isHorizontal = false;
                string keyWord = "";
                string voiceCommand = "";
                int minVelocity = 0;
                int maxVelocity = 0;
                
                if (parsedData[i][0] == "Calibration")
                {
                    joint = parsedData[i][1];
                    swipeMinimalLength = Convert.ToDouble(parsedData[i][2]);
                    swipeMaximalLength = Convert.ToDouble(parsedData[i][3]);
                    swipeMinimalHeight = Convert.ToDouble(parsedData[i][4]);
                    swipeMaximalHeight = Convert.ToDouble(parsedData[i][5]);
                    swipeMinimalDuration = Convert.ToInt32(parsedData[i][6]);
                    swipeMaximalDuration = Convert.ToInt32(parsedData[i][7]);
                    
                    configList.Add(new ConfigContainer(ConvertJointNameToType(joint),
                                                       swipeMinimalLength,
                                                       swipeMaximalLength,
                                                       swipeMinimalHeight,
                                                       swipeMaximalHeight,
                                                       swipeMinimalDuration,
                                                       swipeMaximalDuration,
                                                       eventName,
                                                       methodName,
                                                       configType,
                                                       scale,
                                                       quantityOfNotes,
                                                       isHorizontal,
                                                       keyWord,
                                                       voiceCommand,
                                                       minVelocity,
                                                       maxVelocity));
                }
                else if (parsedData[i][0] == "Scale")
                {
                    for (int k = 1; k < parsedData[i].Count(); k++ )
                    {
                        scale.Add(parsedData[i][k]);
                    }

                    configList.Add(new ConfigContainer(ConvertJointNameToType(joint),
                                                       swipeMinimalLength,
                                                       swipeMaximalLength,
                                                       swipeMinimalHeight,
                                                       swipeMaximalHeight,
                                                       swipeMinimalDuration,
                                                       swipeMaximalDuration,
                                                       eventName,
                                                       methodName,
                                                       configType,
                                                       scale,
                                                       quantityOfNotes,
                                                       isHorizontal,
                                                       keyWord,
                                                       voiceCommand,
                                                       minVelocity,
                                                       maxVelocity));
                }
                else if (parsedData[i][0] == "Note")
                {
                    quantityOfNotes = Convert.ToInt32(parsedData[i][1]);
                    isHorizontal = false;
                    if (parsedData[i][2] == "Horizontal")
                    {
                        isHorizontal = true;
                    }

                    configList.Add(new ConfigContainer(ConvertJointNameToType(joint),
                                                       swipeMinimalLength,
                                                       swipeMaximalLength,
                                                       swipeMinimalHeight,
                                                       swipeMaximalHeight,
                                                       swipeMinimalDuration,
                                                       swipeMaximalDuration,
                                                       eventName,
                                                       methodName,
                                                       configType,
                                                       scale,
                                                       quantityOfNotes,
                                                       isHorizontal,
                                                       keyWord,
                                                       voiceCommand,
                                                       minVelocity,
                                                       maxVelocity));
                }
                else if (parsedData[i][0] == "Velocity")
                {
                    minVelocity = Convert.ToInt32(parsedData[i][1]);
                    maxVelocity = Convert.ToInt32(parsedData[i][2]);

                    configList.Add(new ConfigContainer(ConvertJointNameToType(joint),
                                   swipeMinimalLength,
                                   swipeMaximalLength,
                                   swipeMinimalHeight,
                                   swipeMaximalHeight,
                                   swipeMinimalDuration,
                                   swipeMaximalDuration,
                                   eventName,
                                   methodName,
                                   configType,
                                   scale,
                                   quantityOfNotes,
                                   isHorizontal,
                                   keyWord,
                                   voiceCommand,
                                   minVelocity,
                                   maxVelocity));
                }
                else if (parsedData[i][0] == "Voice")
                {
                    keyWord = parsedData[i][1];
                    voiceCommand = parsedData[i][2];

                    configList.Add(new ConfigContainer(ConvertJointNameToType(joint),
                                                       swipeMinimalLength,
                                                       swipeMaximalLength,
                                                       swipeMinimalHeight,
                                                       swipeMaximalHeight,
                                                       swipeMinimalDuration,
                                                       swipeMaximalDuration,
                                                       eventName,
                                                       methodName,
                                                       configType,
                                                       scale,
                                                       quantityOfNotes,
                                                       isHorizontal,
                                                       keyWord,
                                                       voiceCommand,
                                                       minVelocity,
                                                       maxVelocity));
                }
                else if (parsedData[i][0] == "Event")
                {
                    joint = parsedData[i][1];
                    eventName = parsedData[i][2];
                    methodName = parsedData[i][3];

                    configList.Add(new ConfigContainer(ConvertJointNameToType(joint),
                                                       swipeMinimalLength,
                                                       swipeMaximalLength,
                                                       swipeMinimalHeight,
                                                       swipeMaximalHeight,
                                                       swipeMinimalDuration,
                                                       swipeMaximalDuration,
                                                       eventName,
                                                       methodName,
                                                       configType,
                                                       scale,
                                                       quantityOfNotes,
                                                       isHorizontal,
                                                       keyWord,
                                                       voiceCommand,
                                                       minVelocity,
                                                       maxVelocity));
                }
            }

            return configList;
        }

        /// <summary>
        /// Convert string name of a joint into JointType object
        /// </summary>
        /// <param name="joint">Name of the joint as string value</param>
        /// <returns>By default it returns head</returns>
        private JointType ConvertJointNameToType(string joint)
        {
            // bad idea, but it'll work for now
            switch(joint)
            {
                case "HandRight":
                    return JointType.HandRight;
                case "HandLeft":
                    return JointType.HandLeft;
            }
            return JointType.Head;
        }
        
    }
}
