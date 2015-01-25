using System;
using System.Xml;
using System.Net;

namespace  Gem.Network.Avatar
{
    public class SteamApi
    {
        public SteamApi(string usrname)
        {
            var path = ("http://steamcommunity.com/id/" + usrname + "/?xml=1");
            string xmlStr;

            try
            {
                using (var wc = new WebClient())
                {
                    xmlStr = wc.DownloadString(path);
                }

                var xDoc = new XmlDocument();
                xDoc.LoadXml(xmlStr);

                foreach (XmlNode xNode in xDoc.SelectNodes("profile"))
                {
                    AvatarPath = xNode.SelectSingleNode("avatarFull").InnerText;
                    Name = xNode.SelectSingleNode("steamID").InnerText;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public string AvatarPath
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}
