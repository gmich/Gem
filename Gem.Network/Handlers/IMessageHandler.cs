﻿using System;

namespace Gem.Network.Handlers
{
    /// <summary>
    /// The base interface for handling incoming messages
    /// </summary>
    public interface IMessageHandler
    {
        void Handle(object args);
    }
}
