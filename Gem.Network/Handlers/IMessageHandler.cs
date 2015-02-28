﻿using System;

namespace Gem.Network.Handlers
{
    public interface IMessageHandler
    {
        void Handle(params object[] args);
    }
}
