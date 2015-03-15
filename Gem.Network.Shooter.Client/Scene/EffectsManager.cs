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

        private List<Actor> Effects;
        private Texture2D bullet;
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
            Effects = new List<Actor>();
        }

        #endregion

        #region Initialization

        public void Initialize(ContentManager Content)
        {
            bullet = Content.Load<Texture2D>(@"bullet");
        }

        #endregion

        #region Add Effects Methods

        public void AddBulletParticle(Vector2 location, Vector2 velocity)
        {
        //    Particle particle = new Particle(location, bullet, new Rectangle(0, 0, bullet.Width, bullet.Height), true, velocity , velocity *1000f, 9999f, 100);
        //    Effects.Add(particle);
        }


        #endregion

        #region Helper Methods

        Vector2 randomHorizontalDirection()
        {
            return new Vector2(rand.Next(0, 2000) - 1000, 0);
        }

        Vector2 randomDirection(float scale)
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(rand.Next(0, 100) - 50, rand.Next(0, 100) - 50);
            }
            while (direction.Length() == 0);
            direction.Normalize();
            direction *= scale;

            return direction;
        }

        Vector2 randomNegativeDirection(float scale)
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(rand.Next(0, 20) - 10, rand.Next(45, 60));
            }
            while (direction.Length() == 0);
            direction.Normalize();
            direction *= scale;

            return direction;
        }


        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {          
            for (int x = Effects.Count - 1; x >= 0; x--)
            {
                Effects[x].Update(gameTime);

                if (Effects[x].Enabled)
                {
                    Effects.RemoveAt(x);
                }
            }   
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var effect in Effects)
            {
                effect.Draw(spriteBatch);
            }

        }

        #endregion
              
    }
}