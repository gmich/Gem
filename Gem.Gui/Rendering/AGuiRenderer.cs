using Gem.Gui.Controls;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Gui.Rendering
{    
    public abstract class AGuiRenderer
    {
        protected readonly SpriteBatch batch;

        public AGuiRenderer(SpriteBatch batch)
        {
            this.batch = batch;
        }

        public abstract void Render(AControl component);
    }

    //internal class DrawByRectangle : ABatchDrawable
    //{
    //    public DrawByRectangle(SpriteBatch batch) : base(batch) { }
    //    public override void Draw(AControl component)
    //    {
    //        batch.Draw(component.Sprite.Texture,
    //                   component.Region.Frame,
    //                   component.RenderStyle.Color);
    //    }
    //}

    //internal class DrawByPosition : ABatchDrawable
    //{
    //    public DrawByPosition(SpriteBatch batch) : base(batch) { }

    //    public override void Draw(AControl component)
    //    {
    //        batch.Draw(component.Sprite.Texture,
    //                   component.Region.Position,
    //                   component.RenderStyle.Color);
    //    }
    //}

    //internal class DrawBySourceRectangle : ABatchDrawable
    //{
    //    public DrawBySourceRectangle(SpriteBatch batch) : base(batch) { }

    //    public override void Draw(AControl component)
    //    {
    //        batch.Draw(component.Sprite.Texture,
    //                   component.Region.Frame,
    //                   component.Sprite.SourceRectangle,
    //                   component.RenderStyle.Color);
    //    }
    //}

    //internal class DrawDetailed : ABatchDrawable
    //{
    //    public DrawDetailed(SpriteBatch batch) : base(batch) { }

    //    public override void Draw(AControl component)
    //    {
    //        batch.Draw(component.Sprite.Texture,
    //                   component.Region.Frame,
    //                   component.Sprite.SourceRectangle,
    //                   component.RenderStyle.Color,
    //                   component.RenderStyle.Rotation,
    //                   component.Region.Origin,
    //                   component.RenderStyle.SpriteEffect,
    //                   component.RenderStyle.Layer);
    //    }
    //}

    //TODO: implement spritebatch's overloads ( + 4) 
}
