﻿using Gem.Network.Client;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using System;
using Gem.Network.Extensions;
using Lidgren.Network;

namespace Gem.Network.Handlers
{
    /// <summary>
    /// Handles incoming messages of <see cref="> Gem.Network.Messages.Notification"/>
    /// </summary>
    public class NotificationHandler : IMessageHandler
    {
        public void Handle(NetConnection sender, object obj)
        {
            var args = obj.ReadAllProperties();
            var notificaton = new Notification((string)args[1], (string)args[2]);
            GemClient.ActionManager[GemNetwork.ActiveProfile].OnReceivedNotification(notificaton);
        }
    }

}
