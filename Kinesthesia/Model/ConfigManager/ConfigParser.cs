using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Kinesthesia.Model.ConfigManager
{
    class ConfigParser
    {
        public static IEnumerable<string> ReadLinesFromFile(string filename)
        {
            using (StreamReader reader = new StreamReader(filename))
            {
                while (true)
                {
                    string s = reader.ReadLine();
                    if (s == null)
                        break;
                    yield return s;
                }
            }
        }

        public List<ConfigContainer> ParseSettingsFile(string filename)
        {
            return (from line in ReadLinesFromFile(filename)
                           let item = line.Split(',')
                             select new ConfigContainer()
                           {
                               JointName = item[0],
                               Threshold = Convert.ToInt32(item[1]),
                               EventName = item[2],
                               MethodName = item[3]
                           }).ToList();
        }

        public List<ConfigContainer> ParseSettingsFileXML(string filename)
        {
            XDocument data = XDocument.Parse(filename);

            return (from j in data.Descendants("joint")
                    select new ConfigContainer()
                    {
                        JointName = j.Attribute("type").Value,
                        Threshold = Convert.ToInt32(j.Element("action").Element("threshold").Value),
                        EventName = j.Element("action").Attribute("type").Value,
                        MethodName = j.Element("action").Element("method").Value
                    }).ToList();
        }

        public List<ConfigContainer> ParseSettingsFileTXT(string filename)
        {
            FileInfo fileInfo = new FileInfo(filename);
            StreamReader reader = fileInfo.OpenText();
            string line;
            List<ConfigContainer> resList = new List<ConfigContainer>();
            while ((line = reader.ReadLine()) != null) 
            {
                ConfigContainer config = new ConfigContainer();
                string[] items = line.Split('\t');
                config.JointName = items[0];
                config.Threshold = Convert.ToInt32(items[1]);
                config.EventName = items[2];
                config.MethodName = items[3];
                resList.Add(config);
            }
            return resList;
        }
    }
}
