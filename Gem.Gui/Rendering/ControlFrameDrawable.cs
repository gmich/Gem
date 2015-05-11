using Gem.Gui.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{
    public class ControlFrameDrawable : IControlDrawable
    {
        public void Render(SpriteBatch batch, AControl control)
        {
            batch.Draw(control.Sprite.Texture,
                       control.Region.Frame,
                       control.Sprite.SourceRectangle,
                       control.RenderParameters.Color * control.RenderParameters.Transparency,
                       control.RenderParameters.Rotation,
                       control.RenderParameters.Origin,
                       control.RenderParameters.SpriteEffect,
                       control.RenderParameters.Layer);
        }

    }    
}
