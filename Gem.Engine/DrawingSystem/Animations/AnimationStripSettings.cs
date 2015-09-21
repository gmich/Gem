namespace Gem.DrawingSystem.Animations
{
    public class AnimationStripSettings
    {
        public int FrameWidth { get; }

        public int FrameHeight { get; }

        public byte[] Image { get; set; }

        public string Name { get; }

        public double FrameDelay { get; }

        public int LastFrame { get; }

        public int StartFrame { get; }

        public int TileSheetWidth { get; }

        public int TileSheetHeight { get; }

        public bool Loop { get; }

        public AnimationStripSettings(int frameWidth, int frameHeight,int tilesheetWidth, int tileSheetHeight, string name, double frameDelay,bool loop, byte[] image, int startFrame = 0, int lastFrame = 0)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            TileSheetWidth = tilesheetWidth;
            TileSheetHeight = tileSheetHeight;
            Image = image;
            Name = name;
            FrameDelay = frameDelay;
            LastFrame = lastFrame;
            StartFrame = startFrame;
            Loop = loop;
        }

    }
}
