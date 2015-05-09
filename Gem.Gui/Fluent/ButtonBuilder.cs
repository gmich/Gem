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
        public static AControl Text(this AControl control, string text,SpriteFont font)
        {
            control.Text = new StandardText(font, text);
            return control;
        }

        public static AControl Color(this AControl control, Color color)
        {
            control.RenderParameters.Color = color;
            return control;
        }

        public static AControl TextColor(this AControl control, Color color)
        {
            Contract.Requires(control.Text != null, "Use SetText() before setting the text's color");

            control.Text.RenderParameters.Color = color;
            return control;
        }

        public static AControl OnClick(this AControl control, EventHandler<ControlEventArgs> handler)
        {
            control.Events.Clicked += handler;
            return control;
        }

    }
}
