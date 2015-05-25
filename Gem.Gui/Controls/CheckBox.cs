using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Microsoft.Xna.Framework.Graphics;
using System;
using Gem.Gui.Alignment;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Gem.Gui.Text;

namespace Gem.Gui.Controls
{

    public class CheckBox : AControl
    {
        private readonly Button checkBox;
        private bool isChecked;

        public event EventHandler<bool> CheckChanged;

        public CheckBox(Texture2D texture,
                        Region region,
                        ARenderStyle style,
                        AlignmentContext checkBoxAlignment,
                        Texture2D checkedTexture,
                        Texture2D unCheckedTexture,
                        string text,
                        SpriteFont font)
            : base(texture, region, style)
        {
            int offset = 3;
            this.RenderStyle = style;
            checkBox = new Button(unCheckedTexture, new Region(0, 0, unCheckedTexture.Width, unCheckedTexture.Height), Style.NoStyle, this.Region);
            checkBox.ScreenAlignment.HorizontalAlignment = checkBoxAlignment.HorizontalAlignment;
            checkBox.ScreenAlignment.VerticalAlignment = checkBoxAlignment.VerticalAlignment;
            checkBox.Padding.Left = offset;
            checkBox.ScreenAlignment.Transition = checkBoxAlignment.Transition;
            checkBox.Sprite.Add("checked", checkedTexture);
            this.Events.Clicked += (sender, args) =>
            {
                IsChecked = !IsChecked;
                if (IsChecked) checkBox.Sprite.SwitchSprite("checked");
                else checkBox.Sprite.SwitchSprite();
            };
            Text = new StandardText(font, Vector2.Zero, text);   
            Text.Alignment.HorizontalAlignment = HorizontalAlignment.RelativeTo(() => (checkBox.Region.Frame.Right) + offset * Configuration.Settings.Scale.X);
            Text.Alignment.VerticalAlignment = checkBoxAlignment.VerticalAlignment;
            checkBox.Region.onPositionChange += (args, sender) => Text.Align(this.Region);
        }

        public AControl Box
        {
            get { return checkBox; }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            private set
            {
                isChecked = value;
                ValueChanged();
            }
        }

        internal void ValueChanged()
        {
            var handler = CheckChanged;
            if (handler != null)
            {
                handler(this, isChecked);
            }
        }

        public override IEnumerable<AControl> Entries()
        {
            yield return this;
            //yield return checkBox;
        }

        public override void Align(Region parent)
        {
            base.Align(parent);
            checkBox.Align(this.Region);
        }

        public override void Scale(Vector2 scale)
        {
            base.Scale(scale);
            checkBox.Region.Scale(scale);
            checkBox.RenderParameters.Scale = scale;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
            checkBox.Update(deltaTime);           
        }

        public override void Render(SpriteBatch batch)
        {
            base.Render(batch);
            checkBox.Render(batch);
        }
    }
}
