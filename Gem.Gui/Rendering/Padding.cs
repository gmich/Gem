namespace Gem.Gui.Rendering
{
    public class Padding
    {
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }

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
