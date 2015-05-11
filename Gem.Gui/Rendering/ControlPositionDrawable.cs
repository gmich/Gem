using Gem.Gui.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public class ControlPositionDrawable : IControlDrawable
    {
        public void Render(SpriteBatch batch, AControl control)
        {
            batch.Draw(control.Sprite.Texture,
                       control.Region.Position,
                       control.Sprite.SourceRectangle,
                       control.RenderParameters.Color * control.RenderParameters.Transparency,
                       control.RenderParameters.Rotation,
                       control.Region.Center,
                       control.RenderParameters.Scale,
                       control.RenderParameters.SpriteEffect,
                       control.RenderParameters.Layer);
        }
    }
}
