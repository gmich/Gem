using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Gem.Network.Shooter.Client.Actors;

namespace Gem.Network.Shooter.Client.Scene
{

    public class EffectsManager
    {
        #region Declarations

        private List<Particle> Bullets;
        private ContentManager content;
        private static Random rand;

        #endregion

        #region Instance

        static EffectsManager instance;

        public static EffectsManager GetInstance()
        {
            if (instance == null)
            {
                instance = new EffectsManager();
            }
            return instance;
        }

        private EffectsManager()
        {
            rand = new Random();
            Bullets = new List<Particle>();
        }

        #endregion

        #region Initialization

        public void Initialize(ContentManager content)
        {
            this.content = content;
        }

        #endregion

        #region Add Effects Methods

        public void AddBulletParticle(string name, Vector2 location, Vector2 velocity)
        {
            var particle = new Particle(name, content, location, velocity, 0.0f, 2.0f);
            Bullets.Add(particle);
        }


        #endregion

        #region Collision


        public void BulletIntersects(Actor actor)
        {
            for (int x = Bullets.Count - 1; x >= 0; x--)
            {
                if (Bullets[x].CollisionRectangle.Intersects(actor.CollisionRectangle))
                {
                    //is not friendly
                    if (Bullets[x].Name != actor.Name)
                    {
                        actor.Hit(Bullets[x].Velocity);
                        Bullets.RemoveAt(x);
                    }
                }
            }
        }
        #endregion


        #region Update

        public void Update(GameTime gameTime)
        {
            for (int x = Bullets.Count - 1; x >= 0; x--)
            {
                Bullets[x].Update(gameTime);

                if (!Bullets[x].Enabled)
                {
                    Bullets.RemoveAt(x);
                }
            }   
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in Bullets)
            {
                bullet.Draw(spriteBatch);
            }

        }

        #endregion
              
    }
}