using System;
using System.Net;

namespace  Gem.Network
{
    public class DNSConverter
    {
        public DNSConverter(string DNS)
        {
            if (DNS.Length > 1)
            {
                if (char.IsDigit(DNS[0]))
                    IP = DNS;
                else
                {
                    try
                    {
                        var address = Dns.GetHostAddresses(DNS)[0];
                        IP = address.ToString();
                    }
                    catch (System.Net.Sockets.SocketException exc)
                    {
                        Console.WriteLine(exc.Message);
                        IP = "0";
                    }
                }
                AuthenticateIP();
            }
            else
                IP = "0";
        }

        private void AuthenticateIP()
        {
            string[] parts = IP.Split('.');
            if (parts.Length < 4)
            {
                IP = "0";
            }
            else
            {
                foreach (string part in parts)
                {
                    byte checkPart = 0;
                    if (!byte.TryParse(part, out checkPart))
                    {
                        IP = "0";
                    }
                }
               
            }
        }

        public string IP
        {
            get;
            set;
        }
    }
}