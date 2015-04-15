using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Gem.Gui.Rendering
{

    public class Region
    {
        private bool isRectangleSynced = false;

        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                isRectangleSynced = false;
            }
        }

        private Vector2 size;
        public Vector2 Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
                isRectangleSynced = false;
            }
        }

        private Rectangle rectangle;
        public Rectangle Rectangle
        {
            get
            {
                return isRectangleSynced ?
                    rectangle :
                    rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            }
        }
        
    }

    
}
