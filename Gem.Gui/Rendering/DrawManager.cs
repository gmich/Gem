using Gem.Gui.Elements;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{    
    public abstract class ADrawManager
    {
        protected readonly SpriteBatch batch;

        public ADrawManager(SpriteBatch batch)
        {
            this.batch = batch;
        }

        public abstract void Draw(IGuiComponent element);
    }

    internal class DrawByRectangle : ADrawManager
    {
        public DrawByRectangle(SpriteBatch batch) : base(batch) { }
        public override void Draw(IGuiComponent element)
        {
            batch.Draw(element.Sprite.Texture,
                       element.Region.Frame,
                       element.RenderStyle.Color);
        }
    }

    //internal class DrawByPosition : IDrawManager
    //{
    //    public void Draw(SpriteBatch batch, Region region, RenderStyle renderStyle)
    //    {
    //        batch.Draw(renderStyle.Texture, 
    //                   region.Position, 
    //                   renderStyle.Color);
    //    }
    //}

    //internal class DrawBySourceRectangle : IDrawManager
    //{
    //    public void Draw(SpriteBatch batch, Region region, RenderStyle renderStyle)
    //    {
    //        batch.Draw(renderStyle.Texture, 
    //                   region.Position,
    //                   renderStyle.SourceRectangle,
    //                   renderStyle.Color);
    //    }
    //}

    //internal class DrawDetailed : IDrawManager
    //{
    //    public void Draw(SpriteBatch batch, Region region, RenderStyle renderStyle)
    //    {
    //        batch.Draw(renderStyle.Texture, 
    //                   region.Frame, 
    //                   renderStyle.SourceRectangle,
    //                   renderStyle.Color,
    //                   renderStyle.Rotation,
    //                   region.Origin,
    //                   renderStyle.SpriteEffects,
    //                   renderStyle.Layer);
    //    }
    //}

    //TODO: implement spritebatch's overloads ( + 4) and rename
}
