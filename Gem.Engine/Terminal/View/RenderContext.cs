using Gem.Engine.Containers;
using Microsoft.Xna.Framework.Graphics;
using NullGuard;

namespace Gem.Engine.GTerminal.View
{
    [NullGuard(ValidationFlags.AllPublicArguments)]
    public class RenderContext
    {
        public RenderContext(ContentContainer content, SpriteBatch batch)
        {
            Content = content;
            Batch = batch;
        }
        public ContentContainer Content { get; }
        public SpriteBatch Batch { get; }
    }
}
