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

        public bool Loop { get; }

        public AnimationStripSettings(int frameWidth, int frameHeight, string name, double frameDelay,bool loop, byte[] image, int startFrame = 0, int lastFrame = -1)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            Image = image;
            Name = name;
            FrameDelay = frameDelay;
            LastFrame = lastFrame;
            StartFrame = startFrame;
            Loop = loop;
        }

    }
}
