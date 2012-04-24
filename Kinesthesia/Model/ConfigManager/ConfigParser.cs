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
        /// parsing configuration CSV file at given path
        /// </summary>
        /// <param name="path"></param>
        public List<ConfigContainer> ParseConfigs(string path)
        {
            List<string[]> parsedData = CSVParser.parseCSV(path);

            configList = new List<ConfigContainer>();

            for (int i = 0; i < parsedData.Count(); ++i )
            {
                if (parsedData[i].Count() > 3)
                {
                    string joint = parsedData[i][0];
                    double swipeMinimalLength = Convert.ToDouble(parsedData[i][1]);
                    double swipeMaximalLength = Convert.ToDouble(parsedData[i][2]);
                    double swipeMinimalHeight = Convert.ToDouble(parsedData[i][3]);
                    double swipeMaximalHeight = Convert.ToDouble(parsedData[i][4]);
                    int swipeMinimalDuration = Convert.ToInt32(parsedData[i][5]);
                    int swipeMaximalDuration = Convert.ToInt32(parsedData[i][6]);
                    string eventName = "";
                    string methodName = "";
                    string configType = "Calibration";

                    configList.Add(new ConfigContainer(ConvertJointNameToType(joint),
                                                       swipeMinimalLength,
                                                       swipeMaximalLength,
                                                       swipeMinimalHeight,
                                                       swipeMaximalHeight,
                                                       swipeMinimalDuration,
                                                       swipeMaximalDuration,
                                                       eventName,
                                                       methodName,
                                                       configType));
                }
                else
                {
                    string joint = parsedData[i][0];
                    double swipeMinimalLength = 0.0;
                    double swipeMaximalLength = 0.0;
                    double swipeMinimalHeight = 0.0;
                    double swipeMaximalHeight = 0.0;
                    int swipeMinimalDuration = 0;
                    int swipeMaximalDuration = 0;
                    string eventName = parsedData[i][1];
                    string methodName = parsedData[i][2];
                    string configType = "Event";

                    configList.Add(new ConfigContainer(ConvertJointNameToType(joint),
                                                       swipeMinimalLength,
                                                       swipeMaximalLength,
                                                       swipeMinimalHeight,
                                                       swipeMaximalHeight,
                                                       swipeMinimalDuration,
                                                       swipeMaximalDuration,
                                                       eventName,
                                                       methodName,
                                                       configType));
                }
            }

            return configList;
        }

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
