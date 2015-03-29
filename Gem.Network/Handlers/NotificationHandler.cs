﻿using Gem.Network.Client;
using Gem.Network.Messages;
using Gem.Network.Utilities.Loggers;
using System;
using Gem.Network.Extensions;

namespace Gem.Network.Handlers
{
    /// <summary>
    /// Handles incoming messages of <see cref="> Gem.Network.Messages.Notification"/>
    /// </summary>
    public class NotificationHandler : IMessageHandler
    {
        public void Handle(object obj)
        {
            try
            {
                var args = obj.ReadAllProperties();
                var notificaton = new Notification((string)args[1], (string)args[2]);
                GemClient.ActionManager[GemNetwork.ActiveProfile].OnReceivedNotification(notificaton);
            }
            catch (Exception ex)
            {
                //If an exception occurs, log and move on
                GemNetworkDebugger.Append.Error("Received unhandled notification " + ex.Message);
            }
        }
    }

}
