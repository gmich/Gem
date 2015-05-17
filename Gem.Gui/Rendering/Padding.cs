using Gem.Gui.Configuration;
namespace Gem.Gui.Rendering
{
    public class Padding
    {
        private int top;
        public int Top
        {
            get { return (int)(top * Settings.Scale.Y); }
            set { top = value; }
        }

        private int right;
        public int Right
        {
            get { return (int)(right * Settings.Scale.X); }
            set { right = value; }
        }

        private int bottom;
        public int Bottom
        {
            get { return (int)(bottom * Settings.Scale.Y); }
            set { bottom = value; }
        }

        private int left;
        public int Left
        {
            get { return (int)(left * Settings.Scale.X); }
            set { left = value; }
        }

        public static Padding Zero
        {
            get
            {
                return new Padding
                {
                    Top = 0,
                    Right = 0,
                    Bottom = 0,
                    Left = 0
                };
            }
        }
    }
}
