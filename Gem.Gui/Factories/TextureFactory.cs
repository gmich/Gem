using Gem.Infrastructure.Cache;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Factories
{
    internal class TextureFactory : ITextureFactory
    {
        #region Fields

        private readonly Dictionary<TextureCreationRequest, Texture2D> textureCache =
                     new Dictionary<TextureCreationRequest, Texture2D>(20, new TextureCreationRequestEquality());

        private readonly GraphicsDevice device;

        #endregion

        #region Construct / Dispose

        public TextureFactory(GraphicsDevice device)
        {
            this.device = device;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool isDisposed = false;
        private void Dispose(bool disposing)
        {
            if (disposing && !isDisposed)
            {
                textureCache.Clear();
                isDisposed = true;
            }
        }

        #endregion

        #region ITextureFactory Members

        public Texture2D GetTexture(TextureCreationRequest request)
        {
            Contract.Requires(request != null);
            Texture2D texture;

            if (IsTextureCached(request, out texture))
            {
                return texture;
            }

            texture = new Texture2D(device, request.Width, request.Height, false, SurfaceFormat.Color);
            texture.SetData(request.Pattern.Get(request.Width, request.Height, request.Color));
            textureCache.Add(request, texture);
            
            return texture;
        }

        #endregion

        private bool IsTextureCached(TextureCreationRequest request, out Texture2D texture)
        {
            if(textureCache.ContainsKey(request))
            {
                texture = textureCache[request];
                return true;
            }
            texture = null;

            return false;
        }
    }

    /// <summary>
    /// Used for the GCache lookup
    /// </summary>
    internal class TextureCreationRequestEquality : EqualityComparer<TextureCreationRequest>
    {
        public override int GetHashCode(TextureCreationRequest textureRequest)
        {
            return textureRequest.GetHashCode();
        }

        public override bool Equals(TextureCreationRequest left, TextureCreationRequest right)
        {
            return left.Equals(right);
        }
    }
}
