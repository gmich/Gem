using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Gem.Network.Shooter.Client.Camera;
using Gem.Network.Shooter.Client.Actors;

namespace Gem.Network.Shooter.Client.Scene
{
    public class ParticleManager
    {
        #region Declarations

        List<Particle> particles;
        Texture2D particleTexture;
        Random rand;
        Camera2D camera;
        #endregion

        #region Constructor
        public ParticleManager(ContentManager Content, Camera2D camera)
        {
            this.particleTexture = Content.Load<Texture2D>(@"block");
            particles = new List<Particle>();
            rand = new Random();
            this.camera = camera;
        }

        #endregion

        #region Private Helper Methods

        Vector2 RandomDirection(float scale)
        {
            Vector2 direction;
            do
            {
                direction = new Vector2(rand.Next(0, 100) - 50, rand.Next(0, 100) - 50);
            } while (direction.Length() == 0);

            direction.Normalize();
            direction *= scale;

            return direction;
        }

        Vector2 RandomLocation(int offSet)
        {
            return new Vector2(rand.Next(-offSet, +offSet), rand.Next(-offSet, +offSet));
        }

        #endregion

        #region Add Particles Methods

        public void AddRectangleDestructionParticles(Color color, Vector2 location, int rectWidth, int rectHeight, int particleWidth, int particleHeight)
        {
            for (int x = 0; x < rectWidth / particleWidth; x++)
            {
                for (int y = 0; y < rectHeight / particleHeight; y++)
                {
                    Particle particle = new Particle(location + new Vector2(x * particleWidth, y * particleHeight), particleTexture, new Rectangle(0, 0, particleWidth, particleHeight), RandomDirection((float)rand.Next(10, 20)), RandomDirection((float)rand.Next(10, 20)), 70, 70, color, Color.White);
                    particle.Camera = this.camera;
                    particles.Add(particle);
                }
            }
        }

        #endregion

        #region Update


        public void Update(GameTime gameTime)
        {
            for (int x = particles.Count - 1; x >= 0; x--)
            {
                particles[x].Update(gameTime);
                if (particles[x].Expired)
                {
                    particles.RemoveAt(x);
                }
            }
        }

        #endregion

        #region Draw

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Particle particle in particles)
            {
                particle.Draw(spriteBatch);
            }
        }

        #endregion

    }
}