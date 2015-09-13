namespace Gem.DrawingSystem.Animations
{
    public struct AnimationStripSettings
    {
        public int FrameWidth { get; }

        public int FrameHeight { get; }
        
        public string RelativePathToTexture { get; }

        public string Name { get; }
       
        public double FrameDelay { get; }
        
        public AnimationStripSettings(int frameWidth, int frameHeight, string relativePathToTexture, string name, double frameDelay)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            RelativePathToTexture = relativePathToTexture;
            Name = name;
            FrameDelay = frameDelay;
        }
   
    }
}
