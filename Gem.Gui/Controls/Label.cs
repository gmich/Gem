using Gem.Gui.Styles;
using Gem.Gui.Rendering;
using Gem.Gui.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Gem.Gui.Alignment;
using System.Collections.Generic;

namespace Gem.Gui.Controls
{
    public class Label : AControl
    {
        internal Label(string text,
                     SpriteFont font,
                     Texture2D texture,
                     Region region,
                     Color textcolor,
                     AlignmentContext alignmentContext)
            : base(texture, region, new NoStyle())
        {
            this.Text = new StandardText(font, Vector2.Zero, string.Empty, alignmentContext);
            this.Text.OnTextChanged += (sender, args) => this.Align(Configuration.Settings.ViewRegion);
            this.Text.Value = text;
            this.Text.RenderParameters.Color = textcolor;
            this.Options.IsFocusEnabled = false;
            this.Options.IsHoverEnabled = false;
        }

        private void Stretch(object sender, TextEventArgs args)
        {
            this.Region.Position = new Vector2(Text.Region.Position.X + Text.Padding.Left,
                                     Text.Region.Position.Y + Text.Padding.Top);
            this.Region.Size = new Vector2(Text.Region.Size.X + Text.Padding.Right,
                                     Text.Region.Size.Y + Text.Padding.Bottom);
            this.Region.VirtualSize = this.Region.Size;
        }

        private bool stretchToText;
        public bool StretchToText
        {
            get { return stretchToText; }
            set
            {
                if (stretchToText && !value)
                {
                    this.Text.OnTextChanged -= Stretch;
                }
                if (!stretchToText && value)
                {
                    this.Text.OnTextChanged += Stretch;
                }
                stretchToText = value;
            }
        }

        public override IEnumerable<AControl> Entries()
        {
            yield break;
        }
    }
}
