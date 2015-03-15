using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Gem.Network.Shooter.Client.Level
{
    using Camera;
    using Scene;

    public class TileMap
    {
        #region Declarations

        public int TileWidth;
        public int TileHeight;
        int MapWidth;
        int MapHeight;
        MapSquare[,] mapCells;
        Texture2D tileSheet;
        CameraManager Camera;
        EffectsManager effectsManager;
        
        #endregion

        #region Instance

        static TileMap tileMap;

        public static TileMap GetInstance()
        {
            if (tileMap == null)
                tileMap = new TileMap();

            return tileMap;
        }

        private TileMap()
        {
            Camera = CameraManager.GetInstance();
            effectsManager = EffectsManager.GetInstance();
        }

        #endregion

        #region Initialization

        public void Initialize(Texture2D tileTexture, int tileWidth, int tileHeight)
        {
            tileSheet = tileTexture;
            this.TileWidth = tileWidth;
            this.TileHeight = tileHeight;
        }

        #endregion

        #region Randomize Map
        
        //TODO: update randomize algorithm
        public void Randomize(int mapWidth, int mapHeight)
        {
            this.MapWidth = mapWidth;
            this.MapHeight = mapHeight;

            Random rand = new Random();
            
            mapCells = new MapSquare[MapWidth, MapHeight];

            for (int x = 0; x < MapWidth; x++)
            {

                for (int y = 0; y < MapHeight; y++)
                {
                    if ((y >= 10 || x==0 || x==MapWidth-1))
                    {
                        mapCells[x, y] = new MapSquare(rand.Next(1, 4), false, " ");
                    }
                    else
                    {
                        mapCells[x, y] = new MapSquare();
                    }
                }
            }
        }

        #endregion

        #region Tile and Tile Sheet Handling

        public int TilesPerRow
        {
            get { return tileSheet.Width / TileWidth; }
        }

        public Rectangle TileSourceRectangle(int tileIndex)
        {
            return new Rectangle(
                0,0,
                TileWidth,
                TileHeight);
        }

        #endregion

        #region Information about Map Cells

        public int GetCellByPixelX(int pixelX)
        {
            return pixelX / TileWidth;
        }

        public int GetCellByPixelY(int pixelY)
        {
            return pixelY / TileHeight;
        }

        public Vector2 GetCellByPixel(Vector2 pixelLocation)
        {
            return new Vector2(
                GetCellByPixelX((int)pixelLocation.X),
                GetCellByPixelY((int)pixelLocation.Y));
        }

        public Vector2 GetCellCenter(int cellX, int cellY)
        {
            return new Vector2(
                (cellX * TileWidth) + (TileWidth / 2),
                (cellY * TileHeight) + (TileHeight / 2));
        }

        public Vector2 GetCellCenter(Vector2 cell)
        {
            return GetCellCenter(
                (int)cell.X,
                (int)cell.Y);
        }

        public Rectangle CellWorldRectangle(int cellX, int cellY)
        {
            return new Rectangle(
                cellX * TileWidth,
                cellY * TileHeight,
                TileWidth,
                TileHeight);
        }

        public Rectangle MapToScreenRectangle(int cellX, int cellY)
        {
            return new Rectangle(
            cellX * TileWidth / 4 + (int)Camera.Position.X,
            cellY * TileHeight / 4 + (int)Camera.Position.Y,
            TileWidth / 4,
            TileHeight / 4);
        }

        public Rectangle CellWorldRectangle(Vector2 cell)
        {
            return CellWorldRectangle(
                (int)cell.X,
                (int)cell.Y);
        }

        public Rectangle CellScreenRectangle(int cellX, int cellY)
        {
            return Camera.WorldToScreen(CellWorldRectangle(cellX, cellY));
        }

        public Rectangle CellSreenRectangle(Vector2 cell)
        {
            return CellScreenRectangle((int)cell.X, (int)cell.Y);
        }

        public bool CellIsPassable(int cellX, int cellY)
        {
            MapSquare square = GetMapSquareAtCell(cellX, cellY);

            if (square == null)
                return false;
            else
                return square.Passable;
        }

        public bool CellIsPassable(Vector2 cell)
        {
            return CellIsPassable((int)cell.X, (int)cell.Y);
        }

        public bool CellIsPassableByPixel(Vector2 pixelLocation)
        {
            return CellIsPassable(
                GetCellByPixelX((int)pixelLocation.X),
                GetCellByPixelY((int)pixelLocation.Y));
        }

        public string CellCodeValue(int cellX, int cellY)
        {
            MapSquare square = GetMapSquareAtCell(cellX, cellY);

            if (square == null)
                return "";
            else
                return square.CodeValue;
        }

        public string CellCodeValue(Vector2 cell)
        {
            return CellCodeValue((int)cell.X, (int)cell.Y);
        }
        #endregion

        #region Information about MapSquare objects

        public MapSquare GetMapSquareAtCell(int tileX, int tileY)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                return mapCells[tileX, tileY];
            }
            else
            {
                return null;
            }
        }

        public void SetMapSquareAtCell(
           int tileX,
           int tileY,
           MapSquare tile)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                mapCells[tileX, tileY] = tile;
            }
        }

        public void SetTileAtCell(
           int tileX,
           int tileY,
           int tileIndex)
        {
            if ((tileX >= 0) && (tileX < MapWidth) &&
                (tileY >= 0) && (tileY < MapHeight))
            {
                mapCells[tileX, tileY].LayerTile = tileIndex;
            }
        }

        public MapSquare GetMapSquareAtPixel(int pixelX, int pixelY)
        {
            return GetMapSquareAtCell(
                GetCellByPixelX(pixelX),
                GetCellByPixelY(pixelY));
        }

        public MapSquare GetMapSquareAtPixel(Vector2 pixelLocation)
        {
            return GetMapSquareAtPixel(
                (int)pixelLocation.X,
                (int)pixelLocation.Y);
        }

        public Vector2 GetCellLocation(Vector2 cell)
        {
            return new Vector2(cell.X * TileWidth, cell.Y * TileHeight);
        }

        #endregion

        #region Draw

        private readonly Color Ground = new Color(175, 108, 66);
        private readonly Color GroundDark = new Color(138, 85, 52);
        private readonly Color Sky = new Color(71, 196, 241);
        private Color GetColorById(int id)
        {
            if (id == 0) return Sky;
            if (id == 3) return GroundDark;
            else return Ground;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int startX = GetCellByPixelX((int)Camera.Position.X);
            int endX = GetCellByPixelX((int)Camera.Position.X + Camera.ViewPortWidth);

            int startY = GetCellByPixelY((int)Camera.Position.Y);
            int endY = GetCellByPixelY((int)Camera.Position.Y + Camera.ViewPortHeight);

            for (int x = startX; x <= endX; x++)
                for (int y = startY; y <= endY; y++)
                {
                    if ((x >= 0) && (y >= 0) &&
                        (x < MapWidth) && (y < MapHeight))
                    {
                        spriteBatch.Draw(tileSheet, CellScreenRectangle(x, y), TileSourceRectangle(mapCells[x, y].LayerTile),
                         GetColorById(mapCells[x, y].LayerTile), 0.0f, Vector2.Zero, SpriteEffects.None, 1.0f);
                    }
                }
        }
        
        #endregion

    }
}
