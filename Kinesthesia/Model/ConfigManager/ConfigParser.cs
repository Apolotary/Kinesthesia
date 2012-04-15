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
                string joint = parsedData[i][0];
                int threshold = Convert.ToInt32(parsedData[i][1]);
                string eventName = parsedData[i][2];
                string methodName = parsedData[i][3];

                configList.Add(new ConfigContainer(ConvertJointNameToType(joint), threshold, eventName, methodName));
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
