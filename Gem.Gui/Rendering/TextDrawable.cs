using Gem.Gui.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public class TextDrawable : ITextDrawable
    {
        public void Render(SpriteBatch batch, IText text)
        {
            batch.DrawString(text.Font,
                             text.Value,
                             text.Region.Position,
                             text.RenderParameters.Color,
                             text.RenderParameters.Rotation,
                             text.RenderParameters.Origin,
                             text.RenderParameters.Scale,
                             text.RenderParameters.SpriteEffect,
                             text.RenderParameters.Layer);

        }
    }
}
