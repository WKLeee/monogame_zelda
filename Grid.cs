using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;

namespace Sprint_1
{
    //Class to hold location of all sprites on the map.
    //NOT constantly checking each for intercepts, just tracking locations
    public class Grid
    {
        private Vector2 myBlockSize;
        private readonly Collection<ISprite>[][] spritesInCell; //Tracks what sprites are in what cells
        private int viewportWidthInCells;
        private int viewportHeightInCells;
        private int totalWidth;
        private int totalHeight;
        private int widthFactor;
        private int heightFactor;
        private int levelWidthMultiplier;

        public Grid(int width, int height, int widthMultiple, int heightMultiple, int levelSize) //Singleton
        {
            myBlockSize = new Vector2(16, 16);
            spritesInCell = new Collection<ISprite>[height + 1][];
            levelWidthMultiplier = levelSize;
            totalWidth = width;
            totalHeight = height;
            widthFactor = widthMultiple;
            heightFactor = heightMultiple;
            viewportWidthInCells = width / 16;
            viewportHeightInCells = height / 16;
            for (int i = 0; i < height + 1; i++)
            {
                spritesInCell[i] = new Collection<ISprite>[width + 1];
                for (int j = 0; j < width + 1; j++)
                {
                    spritesInCell[i][j] = new Collection<ISprite>();
                }
            }
        }

        public int LevelWidthMultiplier
        {
            get { return levelWidthMultiplier; }
            set { levelWidthMultiplier = value; }
        }

        public int WidthInCells
        {
            get { return viewportWidthInCells; }
            set { viewportWidthInCells = value;  }
        }

        //Adds a sprite to a cell given the sprite's cell.
        public void AddSpriteToCell(Vector2 cell, ISprite sprite)
        {
            spritesInCell[(int)cell.Y][(int)cell.X].Add(sprite);
        }

        //Gets the list of all sprites in a cell.
        public Collection<ISprite> GetSpritesInCell(Vector2 cell)
        {
            return spritesInCell[(int)cell.Y][(int)cell.X];
        }

        //Removes a sprite from cell's list. Need to use when sprite moves out of cell.
        public void RemoveSpriteFromCell(Vector2 vector, ISprite sprite)
        {
            if (spritesInCell[(int)vector.Y][(int)vector.X].Contains(sprite))
            {
                spritesInCell[(int) vector.Y][(int) vector.X].Remove(sprite);
            }
        }

        public int HeightInCells
        {
            get { return viewportHeightInCells; }
            set { viewportHeightInCells = value; }
        }

        public int TotalWidth
        {
            get { return totalWidth; }
            set { totalWidth = value; }
        }

        public int TotalHeight
        {
            get { return totalHeight; }
            set { totalHeight = value; }
        }

        public int WidthFactor
        {
            get { return widthFactor; }
            set { widthFactor = value; }
        }

        public int HeightFactor
        {
            get { return heightFactor; }
            set { heightFactor = value; }
        }

        public static Vector2 ComputeNextCell(ISprite sprite)
        {
            Vector2 proposedPosition = new Vector2(sprite.MovingPosition.X, sprite.MovingPosition.Y);
            proposedPosition += new Vector2(sprite.Velocity.X, sprite.Velocity.Y);          
            return proposedPosition;
        }       

        public Vector2 BlockSize
        {
            get { return myBlockSize; }
            set { myBlockSize = value; }
        }
        
    }
}
