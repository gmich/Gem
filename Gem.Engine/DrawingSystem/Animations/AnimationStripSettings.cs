namespace Gem.DrawingSystem.Animations
{
    public class AnimationStripSettings
    {
        public int FrameWidth { get; }

        public int FrameHeight { get; }

        public string Path { get; }

        public string Name { get; }

        public double FrameDelay { get; }

        public int LastFrame { get; }

        public int StartFrame { get; }

        public bool Loop { get; }

        public AnimationStripSettings(int frameWidth, int frameHeight, string path, string name, double frameDelay,bool loop, int startFrame = 0, int lastFrame = -1)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            Path = path;
            Name = name;
            FrameDelay = frameDelay;
            LastFrame = lastFrame;
            StartFrame = startFrame;
            Loop = loop;
        }

    }
}
