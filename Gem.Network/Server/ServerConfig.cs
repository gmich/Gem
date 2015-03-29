namespace Gem.Network.Server
{
    /// <summary>
    /// The server configuration. 
    /// </summary>
    public class ServerConfig
    {

        public string Name { get; set; }

        public int Port { get; set; }

        public string Password { get; set; }

        public int MaxConnections { get; set; }

        public int MaxConnectionAttempts { get; set; }

        public bool RequireAuthentication { get; set; }

        public float ConnectionTimeout { get; set; }

        public bool EnableUPnP { get; set; }

        public string DisconnectMessage { get; set; }

    }
}
