using Gem.Gui.Alignment;
using Gem.Gui.Controls;
using Gem.Gui.Core.Controls;
using Gem.Gui.Layout;
using Gem.Gui.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics.Contracts;

namespace Gem.Gui.Fluent
{
    public static class ControlFluentBuilder
    {
        public static AControl Text(this AControl control, SpriteFont font, string text, int x = 0, int y = 0, bool relativeToParent = false)
        {
            control.Text = new StandardText(font, control.Region.Position + new Vector2(x, y), text);

            if (relativeToParent)
            {
                control.Text.Alignment = new AlignmentContext(HorizontalAlignment.RelativeTo(() => control.Region.Position.X, x),
                                                              VerticalAlignment.RelativeTo(() => control.Region.Position.Y, y),
                                                              AlignmentTransition.Fixed);
            }
            return control;
        }

        public static AControl Color(this AControl control, Color color)
        {
            control.RenderParameters.Color = color;
            return control;
        }

        public static AControl Padding(this AControl control, int top = 0, int bottom = 0, int left = 0, int right = 0)
        {
            control.Padding.Top = top;
            control.Padding.Bottom = bottom;
            control.Padding.Left = left;
            control.Padding.Right = right;

            return control;
        }

        public static AControl TextColor(this AControl control, Color color)
        {
            Contract.Requires(control.Text != null, "Use Text() before setting the text's color");

            control.Text.RenderParameters.Color = color;
            return control;
        }

        public static AControl Sprite(this AControl control, string tag, Texture2D texture, Rectangle? sourceRectangle = null)
        {
            control.Sprite.Add(tag, texture, sourceRectangle);
            control.Sprite.SwitchSprite(tag);
            return control;
        }

        public static AControl TextField(this AControl control, Color color)
        {
            Contract.Requires(control.Text != null, "Use Text() before setting the text's color");

            control.Text.RenderParameters.Color = color;
            return control;
        }
        public static AControl ScreenAlignment(this AControl listView, IHorizontalAlignable horizontalAignment, IVerticalAlignable verticalAignment)
        {
            listView.ScreenAlignment.HorizontalAlignment = horizontalAignment;
            listView.ScreenAlignment.VerticalAlignment = verticalAignment;
            return listView;
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

        public static TextField OnTextEntry(this TextField textField, EventHandler<string> handler)
        {
            textField.OnTextEntered += handler;
            return textField;
        }

        public static AControl OnClick(this AControl control, EventHandler<ControlEventArgs> handler)
        {
            control.Events.Clicked += handler;
            return control;
        }

    }
}
