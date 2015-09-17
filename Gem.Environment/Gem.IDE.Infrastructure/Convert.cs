namespace Gem.IDE.Infrastructure
{
    public static class Convert
    {
        public static Microsoft.Xna.Framework.Point ToMonogamePoint(this System.Windows.Point point)
        {
            return new Microsoft.Xna.Framework.Point((int)point.X, (int)point.Y);
        }
    }
}
