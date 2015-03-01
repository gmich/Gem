using System;

namespace Gem.Network.Fluent
{
    public class ProfileRouter
    {
        private readonly string profile;

        public ProfileRouter(string profile)
        {
            this.profile = profile;
        }

        public IClientMessageRouter Client
        {
            get
            {
                return new ClientMessageRouter(profile);
            }
        }

        public IServerMessageRouter Server
        {
            get
            {
                return new ServerMessageRouter(profile);
            }
        }
    }
}
