using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace EdgeEditLib
{
    public static class XmlHelper
    {
        public static void AddLevelToList(string levelName, LevelType levelType)
        {
            if (LevelParser.GamePath.Length == 0)
            {
                return;
            }
            string path = Path.Combine(LevelParser.GamePath, "levels", "mapping.xml");
            XmlDocument xml = new XmlDocument();
            xml.Load(path);
            XmlElement xRoot = xml.DocumentElement;
            if (xRoot == null)
            {
                return;
            }

            XmlNode typeNode = xml.SelectSingleNode("/levels/" + levelType.ToString().ToLower());

            bool levelExists = false;
            foreach (XmlNode levelNode in typeNode)
            {
                if (levelNode.Attributes["filename"].Value == levelName)
                {
                    levelExists = true;
                }
            }

            if (levelExists)
            {
                return;
            }

            XmlElement levelElement = xml.CreateElement("level");
            levelElement.SetAttribute("filename", levelName);
            levelElement.SetAttribute("leaderboard_id", "-1");
            levelElement.SetAttribute("name_sfx", "");

            // or AppendChild is better?
            typeNode.PrependChild(levelElement);

            xml.Save(path);

        }
    }
}
