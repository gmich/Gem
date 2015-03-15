using Gem.Network.Client;
using Gem.Network.Messages;
using Gem.Network.Shooter.Client.Actors;
using Gem.Network.Utilities.Loggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Gem.Network.Shooter.Client.Scene
{
    public class EventManager
    {
        private readonly ContentManager content;
        private Dictionary<string,Actor> actors;
        private Dictionary<string, Action<string>> commandTable;

        private readonly GemClient client;

        public EventManager(ContentManager content,string name)
        {
            this.content = content;
            this.actors = new Dictionary<string, Actor>();
            this.commandTable = new Dictionary<string,Action<string>>();
            GemNetworkDebugger.Echo = Console.WriteLine;
            RegisterCommands();

            //client = new GemClient("GemChat", "GemChat",  "83.212.103.13",, 14242, name);
            client = new GemClient("GemChat", new ConnectionConfig
            {
                ServerName = "GemChat",
                IPorHost = "127.0.0.1",
                Port = 14242,
                DisconnectMessage = name
            }, PackageConfig.TCP);

            GemClient.Profile("GemChat").OnReceivedServerNotification(x => 
                {
                    if (x.Type == NotificationType.Command)
                    {
                        var command = x.Message.Split(' ')[0];
                        if (commandTable.ContainsKey(command))
                            commandTable[command](command);
                    }
                });
        }

        private void RegisterCommands()
        {
            commandTable.Add("newplayer", x =>
            {
                var name = x.Split(' ');
                AddActor(name[1], new Vector2(1500, 10));
            });

            commandTable.Add("removeplayer", x =>
            {
                var name = x.Split(' ');
                RemoveActor(name[1]);
            });
        }

        public void AddActor(string id, Vector2 location)
        {
            if (!actors.ContainsKey(id))
            {
                actors.Add(id, new Actor(content, location));
            }
        }

        public void RemoveActor(string id)
        {
            if (actors.ContainsKey(id))
            {
                actors.Remove(id);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach(var actor in actors)
            {
                actor.Value.Update(gameTime);
            }
        }

        public void SetLocation(string id, Vector2 location)
        {
            if(actors.ContainsKey(id))
            {
                actors[id].WorldLocation = location;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var actor in actors)
            {
                actor.Value.Draw(spriteBatch);
            }
        }
    }
}
