namespace Gem.IDE.Infrastructure
{
    public static class Convert
    {
        public static Microsoft.Xna.Framework.Point ToMonogamePoint(this System.Windows.Point point)
        {
            return new Microsoft.Xna.Framework.Point((int)point.X, (int)point.Y);
        }

        public static Microsoft.Xna.Framework.Vector2 ToMonogameVector2(this System.Windows.Point point)
        {
            return new Microsoft.Xna.Framework.Vector2((float)point.X, (float)point.Y);
        }

        public static Microsoft.Xna.Framework.Vector2 ToMonogameVector2(this System.Windows.Vector point)
        {
            return new Microsoft.Xna.Framework.Vector2((float)point.X, (float)point.Y);
        }
    }
}
