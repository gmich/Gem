﻿using Gem.Network.Utilities.Loggers;
using System;

namespace Gem.Network.Handlers
{
    public class NotificationHandler : IMessageHandler
    {
        public void Handle(params object[] args)
        {
            if (args.Length > 1)
            {
                GemNetworkDebugger.Echo((string)args[1]);
            }
        }
    }

}
