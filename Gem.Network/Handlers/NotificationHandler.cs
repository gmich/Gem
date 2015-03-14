﻿using Gem.Network.Client;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using System;

namespace Gem.Network.Handlers
{
    public class NotificationHandler : IMessageHandler
    {
        public void Handle(params object[] args)
        {
            try
            {
                var notificaton = new Notification((string)args[1], (string)args[2]);
                GemClient.ActionManager[GemNetwork.ActiveProfile].OnReceivedNotification(notificaton);
            }
            catch (Exception ex)
            {
                GemNetworkDebugger.Echo("Received unhandled notification " + ex.Message);
            }
        }
    }

}
