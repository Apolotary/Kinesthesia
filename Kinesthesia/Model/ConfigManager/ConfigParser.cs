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
                if (parsedData[i][0] == "Calibration")
                {
                    string joint = parsedData[i][1];
                    double swipeMinimalLength = Convert.ToDouble(parsedData[i][2]);
                    double swipeMaximalLength = Convert.ToDouble(parsedData[i][3]);
                    double swipeMinimalHeight = Convert.ToDouble(parsedData[i][4]);
                    double swipeMaximalHeight = Convert.ToDouble(parsedData[i][5]);
                    int swipeMinimalDuration = Convert.ToInt32(parsedData[i][6]);
                    int swipeMaximalDuration = Convert.ToInt32(parsedData[i][7]);
                    string eventName = "";
                    string methodName = "";
                    string configType = "Calibration";
                    List<string> scale = new List<string>();
                    int quantityOfNotes = 0;
                    bool isHorizontal = false;
                    string keyWord = "";
                    string voiceCommand = "";

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
                                                       voiceCommand));
                }
                else if (parsedData[i][0] == "Scale")
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
                    string configType = "Scale";
                    List<string> scale = new List<string>();
                    int quantityOfNotes = 0;
                    bool isHorizontal = false;
                    string keyWord = "";
                    string voiceCommand = "";

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
                                                       voiceCommand));
                }
                else if (parsedData[i][0] == "Note")
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
                    string configType = "Note";
                    List<string> scale = new List<string>();
                    int quantityOfNotes = Convert.ToInt32(parsedData[i][1]);
                    bool isHorizontal = false;
                    if (parsedData[i][2] == "Horizontal")
                    {
                        isHorizontal = true;
                    }
                    string keyWord = "";
                    string voiceCommand = "";

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
                                                       voiceCommand));
                }
                else if (parsedData[i][0] == "Voice")
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
                    string configType = "Voice";
                    List<string> scale = new List<string>();
                    int quantityOfNotes = 0;
                    bool isHorizontal = false;
                    string keyWord = parsedData[i][1];
                    string voiceCommand = parsedData[i][2];

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
                                                       voiceCommand));
                }
                else if (parsedData[i][0] == "Event")
                {
                    string joint = parsedData[i][1];
                    double swipeMinimalLength = 0.0;
                    double swipeMaximalLength = 0.0;
                    double swipeMinimalHeight = 0.0;
                    double swipeMaximalHeight = 0.0;
                    int swipeMinimalDuration = 0;
                    int swipeMaximalDuration = 0;
                    string eventName = parsedData[i][2];
                    string methodName = parsedData[i][3];
                    string configType = "Event";
                    List<string> scale = new List<string>();
                    int quantityOfNotes = 0;
                    bool isHorizontal = false;
                    string keyWord = "";
                    string voiceCommand = "";

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
                                                       voiceCommand));
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
