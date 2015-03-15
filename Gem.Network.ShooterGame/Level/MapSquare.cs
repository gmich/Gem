using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Network.Shooter.Client.Level
{
    public class MapSquare
    {
        #region Public Constructors

        public MapSquare()
        {
            this.Passable = true;
            this.CodeValue = "";
            this.LayerTile = 0;
        }

        public MapSquare(int layerTile, bool passable, string codeValue)
        {
            this.Passable = passable;
            this.CodeValue = codeValue;
            this.LayerTile = layerTile;
        }

        #endregion

        #region Properties

        public int LayerTile
        {
            get;
            set;
        }

        public bool Passable
        {
            get;
            set;
        }

        public string CodeValue
        {
            get;
            set;
        }

        #endregion
    }
}
