using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gem.Gui.Text
{
    public class TextEventArgs : EventArgs
    {
        private readonly string oldText;
        private readonly string text;

        public TextEventArgs(string text, string oldText)
        {
            this.text = text;
            this.oldText = oldText;
        }

        public string OldText { get { return oldText; } }
        public string Text { get { return text; } }
    }

    public class TextStyle
    {
        private readonly SpriteFont font;
        private string text;

        public TextStyle(string text, SpriteFont font)
        {
            this.font = font;
            this.text = text;
        }

        public EventHandler<TextEventArgs> TextChanged { get; set; }

        public SpriteFont Font { get { return font; } }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                OnTextChanged(new TextEventArgs(text: value, oldText: text));
                text = value;
            }
        }

        private void OnTextChanged(TextEventArgs args)
        {
            var handler = TextChanged;
            if (handler != null)
            {
                handler(this, args);
            }
        }
    }
}
