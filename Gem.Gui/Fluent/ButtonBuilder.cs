using Gem.Gui.Alignment;
using Gem.Gui.Controls;
using Gem.Gui.Core.Controls;
using Gem.Gui.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Contracts;

namespace Gem.Gui.Fluent
{
    public static class ControlFluentBuilder
    {
        public static AControl Text(this AControl control, string text, int x, int y, SpriteFont font)
        {
            control.Text = new StandardText(font, control.Region.Position + new Vector2(x, y), text);
            return control;
        }

        public static AControl Color(this AControl control, Color color)
        {
            control.RenderParameters.Color = color;
            return control;
        }

        public static AControl TextColor(this AControl control, Color color)
        {
            Contract.Requires(control.Text != null, "Use Text() before setting the text's color");

            control.Text.RenderParameters.Color = color;
            return control;
        }

        public static AControl TextHorizontalAlignment(this AControl control, IHorizontalAlignable horizontalAignment)
        {
            Contract.Requires(control.Text != null, "Use Text() before setting the text's color");

            control.Text.Alignment.HorizontalAlignment = horizontalAignment;
            return control;
        }

        public static AControl TextVerticalAlignment(this AControl control, IVerticalAlignable verticalAignment)
        {
            Contract.Requires(control.Text != null, "Use Text() before setting the text's color");

            control.Text.Alignment.VerticalAlignment = verticalAignment;
            return control;
        }

        public static AControl OnClick(this AControl control, EventHandler<ControlEventArgs> handler)
        {
            control.Events.Clicked += handler;
            return control;
        }

    }
}
