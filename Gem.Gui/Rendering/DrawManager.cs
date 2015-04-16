using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{    
    public interface IDrawManager
    {
        void Draw(SpriteBatch batch, Region region, RenderStyle renderStyle);
    }
    
    internal class DrawByRectangle : IDrawManager
    {
        public void Draw(SpriteBatch batch, Region region, RenderStyle renderStyle)
        {
            batch.Draw(renderStyle.Texture, 
                       region.Frame, 
                       renderStyle.Color);
        }
    }

    internal class DrawByPosition : IDrawManager
    {
        public void Draw(SpriteBatch batch, Region region, RenderStyle renderStyle)
        {
            batch.Draw(renderStyle.Texture, 
                       region.Position, 
                       renderStyle.Color);
        }
    }

    internal class DrawBySourceRectangle : IDrawManager
    {
        public void Draw(SpriteBatch batch, Region region, RenderStyle renderStyle)
        {
            batch.Draw(renderStyle.Texture, 
                       region.Position,
                       renderStyle.SourceRectangle,
                       renderStyle.Color);
        }
    }

    internal class DrawDetailed : IDrawManager
    {
        public void Draw(SpriteBatch batch, Region region, RenderStyle renderStyle)
        {
            batch.Draw(renderStyle.Texture, 
                       region.Frame, 
                       renderStyle.SourceRectangle,
                       renderStyle.Color,
                       renderStyle.Rotation,
                       region.Origin,
                       renderStyle.SpriteEffects,
                       renderStyle.Layer);
        }
    }

    //TODO: implement spritebatch's overloads ( + 4) and rename
}
