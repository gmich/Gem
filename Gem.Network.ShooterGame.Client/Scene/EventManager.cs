using Gem.Network.Client;
using Gem.Network;
using Gem.Network.Messages;
using Gem.Network.Shooter.Client.Actors;
using Gem.Network.Utilities.Loggers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Lidgren.Network;
using System.Collections.Generic;

namespace Gem.Network.Shooter.Client.Scene
{
    public class EventManager
    {
        private readonly ContentManager content;
        private Dictionary<string,Actor> actors;
        private Dictionary<string, Action<string>> commandTable;
        private GameTime gameTime;
        private readonly GemClient client;

        public EventManager(ContentManager content,string name)
        {
            this.content = content;
            this.actors = new Dictionary<string, Actor>();
            this.commandTable = new Dictionary<string,Action<string>>();
            GemNetworkDebugger.Echo = Console.WriteLine;
            RegisterCommands();

            //client = new GemClient("GemChat", "GemChat",  "83.212.103.13",, 14242, name);
            client = new GemClient("Shooter", new ConnectionConfig
            {
                ServerName = "Shooter",
                IPorHost = "83.212.103.13",
                Port = 14242,
                DisconnectMessage = name
            }, PackageConfig.UDPSequenced);

            GemClient.Profile("Shooter").OnReceivedServerNotification(x => 
                {
                    if (x.Type == NotificationType.Command)
                    {
                        var command = x.Message.Split(' ')[0];
                        if (commandTable.ContainsKey(command))
                            commandTable[command](x.Message);
                    }
                    if (x.Type == NotificationType.Message)
                    {
                        Console.WriteLine(x.Message);
                    }
                });

            client.RunAsync(() => new ConnectionApprovalMessage { Message = "Incoming client", Sender = name, Password = "none" });
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
                actors.Add(id, new Actor(id,content, location));
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
            this.gameTime=gameTime;
            foreach(var actor in actors)
            {
                actor.Value.Update(gameTime);
            }
        }

        public void SetLocation(string id, float x, float y)
        {
            if (actors.ContainsKey(id))
            {
                var newX = Interpolate(x, actors[id].WorldLocation.X);
                var newY = Interpolate(y, actors[id].WorldLocation.Y);
                actors[id].WorldLocation = new Vector2(newX, newY);
            }
        }

        private const float threshold = 1.0f;
        private const float interpolationConstant = 0.3f;
        private float Interpolate(float remotePosition, float localPosition)
        {
            //  var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var difference = remotePosition - localPosition;

            if (Math.Abs(difference) < threshold)
            {
                Console.WriteLine(String.Format(@"AbsDifference:{0} RawDifference:{1} Remote:{2} Local{3}", 
                                  Math.Abs(difference), difference, remotePosition, localPosition));
                return remotePosition;
            }
            else
            {
                return localPosition + (difference * interpolationConstant);
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
