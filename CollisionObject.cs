using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable PossibleLossOfFraction

namespace Sprint_1
{
    //Will be used in each sprite class. Contains collision objects and methods used by individual sprites
    public class CollisionObject
    {        
        private readonly ISprite mySprite;
        private ISprite oldSprite;
        private Rectangle boundingBox;
        private bool colliding; //If the object is colliding with something else
        private static bool _visible; //If the collision should be visible - static variable
        private Color color;
        private bool boundaryX, boundaryY;
        private List<Vector2> gridPosition; //Keeps track of the sprite's current cells
        private List<Vector2> oldPosition; //Keeps track of the sprite's previous cells
        Grid grid; //The map, containing list of where every sprite is located
        private Collider collider;
        private static Texture2D _myPixel;
        private ISprite collidingSprite;
        private bool collisionOn;
        private static GraphicsDeviceManager _graphics; //Only ever 1 graphics
        
        //Constructor
        public CollisionObject(ISprite sprite, Grid board, Collider universal)
        {
            //All blocks are 16x16 pixels            
            grid = board;
            oldSprite = new NullClass();
            collider = universal;
            mySprite = sprite;
            colliding = false;
            boundaryX = false;
            boundaryY = false;
            _visible = false;            
            color = DetermineColor();
            DetermineBoxSize();
            gridPosition = new List<Vector2>();
            collisionOn = true;
            DetermineGridPosition();      
            AddToMoving();
        }  
        public Rectangle CollisionBox
        {
            get { return boundingBox; }
            set { boundingBox = value; }
        }

        //Only need one graphics, and only need to update it once. So this method can be static since it isn't needed by
        //any one specific object
        public static void UpdateGraphics(GraphicsDeviceManager device)
        {
            _graphics = device;
        }

        public void SetCollidingSprite()
        {
            collidingSprite = new NullClass();
        }

        public static bool IsVisible
        {
            get { return _visible; }
            set { _visible = value; }
        }

        public bool IsColliding
        {
            get { return colliding; }
            set { colliding = value; }
        }

        public bool CollisionOn
        {
            get { return collisionOn; }
            set { collisionOn = value; }
        }
        //Figures out what cells the sprite is currently occupying
        public void DetermineGridPosition()
        {
            oldPosition = new List<Vector2>(gridPosition);
            gridPosition = new List<Vector2>(); //Creates a new list each time so we don't have to remove objects
                                                // after moving to different cell
            double[] coords = mySprite.GridPosition();
            int widthInt = ConvertToInt(coords[0]);
            int heightInt = ConvertToInt(coords[1]);
            int locationXInt = ConvertToInt(coords[2]);
            int locationYInt = ConvertToInt(coords[3]);
            GetCells(locationXInt, locationYInt, widthInt, heightInt); //Adds these cells to current position list
        }

        public static int ConvertToInt(double num)
        {
            if (num % 1 == 0 && num != 0) //Second part needed to avoid negatives
            {
                num -= 1;
            }
            return (int)num;
        }

        //Adds the sprite to the grid. Also, this object tracks it's own position.
        private void AddToGrid(Vector2 vector)
        {
            if (!gridPosition.Contains(vector)) //In case the object is in less than 4 cells
            {
                gridPosition.Add(vector);               
            }

            //Adds sprite to cell in grid list
            if (!grid.GetSpritesInCell(vector).Contains(mySprite))
            {
                grid.AddSpriteToCell(vector, mySprite);
            }
        }

        //If a sprite spans more than 2 cells in height, need to check.
        private void HeightCheck(int x, int y, int width, int height)
        {
            while (height > y || y > height)
            {
                Vector2 blockExtra1, blockExtra2;
                if (height > y)
                {
                    height--;
                    blockExtra1 = new Vector2(width, height);
                    blockExtra2 = new Vector2(x, height);
                }
                else
                {
                    y--;
                    blockExtra1 = new Vector2(width, y);
                    blockExtra2 = new Vector2(x, y);
                }
                AddToGrid(blockExtra1);
                AddToGrid(blockExtra2);
            }
        }

        //If a sprite spans more than 2 cells in width, need to check
        private void WidthCheck(int x, int y, int width, int height)
        {
            while (height > y || y > height)
            {
                Vector2 blockExtra1, blockExtra2;
                if (height > y)
                {
                    height--;
                    blockExtra1 = new Vector2(width, height);
                    blockExtra2 = new Vector2(x, height);
                }
                else
                {
                    y--;
                    blockExtra1 = new Vector2(width, y);
                    blockExtra2 = new Vector2(x, y);
                }
                AddToGrid(blockExtra1);
                AddToGrid(blockExtra2);
            }
        }

        //Creates all of the cells the sprite COULD be in. AddToGrid manages if these cells are correct.
        //y is taller than height
        private void GetCells(int x, int y, int width, int height)
        {
            Vector2 block1 = new Vector2(width, height);
            Vector2 block2 = new Vector2(width, y);
            Vector2 block3 = new Vector2(x, height);
            Vector2 block4 = new Vector2(x, y);

           //A single cell could have up to 4 neighboring cells (Less if a sprite spans more than one cell)
           AddToGrid(block1);
           AddToGrid(block2);
           AddToGrid(block3);
           AddToGrid(block4);
           //Some sprites could have more than 4 cells (Like a flag). Use these next two methods to check
           WidthCheck(x, y, width, height);
           HeightCheck(x, y, width, height);
        }


        //Creates the bounding box. Different sizes for PlayerSprite and BlockSprite, EnemySprite, ItemSprites
        public void DetermineBoxSize()
        {
            boundingBox = mySprite.BoxSize();
        }

        //Sets the color for the bounding box
        private Color DetermineColor()
        {
            return mySprite.BoxColor;
        }

        //Shows/hides bounding box
        public static void ToggleVisibility(Texture2D pixel)
        {
            _myPixel = pixel;
            _visible = !_visible;
        }

        //Draw outline of boundary box
        public void DrawOutline(SpriteBatch spriteBatch, int thickness, Color type)
        {
            if (spriteBatch != null)
            {
                //Top line
                spriteBatch.Draw(_myPixel,
                    new Rectangle(boundingBox.X, boundingBox.Y, boundingBox.Width, thickness), type);
                //Left line
                spriteBatch.Draw(_myPixel,
                    new Rectangle(boundingBox.X, boundingBox.Y, thickness, boundingBox.Height), type);
                //Right line
                spriteBatch.Draw(_myPixel, new Rectangle((boundingBox.X + boundingBox.Width - thickness)
                    , boundingBox.Y, thickness, boundingBox.Height), type);
                //Left line
                spriteBatch.Draw(_myPixel, new Rectangle(boundingBox.X,
                    (boundingBox.Y + boundingBox.Height - thickness),
                    boundingBox.Width, thickness), type);
            }
        }
      

        //Adds all sprites in neighboring cell to adjacent list
        public void AddToNeighborList(Vector2 cell, Collection<ISprite> adj)
        {
                Collection<ISprite> spritesInCell = grid.GetSpritesInCell(cell);
                //Adds every sprite in the grid's list for cell
                foreach (ISprite sprite in spritesInCell)
                {
                    adj.Add(sprite);
                }
        }

        //Creates neighbor cells adjacent to cell to check for sprites. If so, need to be aware to check them.
        //1 cell can neighbor up to 4 cells.
        public void CheckNeighboringCell(Vector2 cell, Collection<ISprite> adjacent)
        {
            Vector2 cellBack, cellForward, cellUp, cellDown;

                if ((int)cell.X != 0)
                {
                    int minus = (int)cell.X - 1;
                    cellBack = new Vector2(minus, cell.Y);  
                    AddToNeighborList(cellBack, adjacent); //Adds to list if cell isn't the same cell as one being checked
                }

                if ((int)cell.X != grid.WidthInCells)
                {
                    int plus = (int) cell.X + 1;
                    cellForward = new Vector2(plus, cell.Y);
                    AddToNeighborList(cellForward, adjacent); //Adds to list if cell isn't the same cell as one being checked
            }

                if ((int)cell.Y != 0)
                {
                    int down = (int)cell.Y - 1;
                    cellDown = new Vector2(cell.X, down);
                    AddToNeighborList(cellDown, adjacent); //Adds to list if cell isn't the same cell as one being checked
            }

                if ((int) cell.Y != grid.HeightInCells)
                {
                    int up = (int) cell.Y + 1;
                    cellUp = new Vector2(cell.X, up);
                    AddToNeighborList(cellUp, adjacent); //Adds to list if cell isn't the same cell as one being checked
            }          
        }

        //Used for objects inside other items
        public void MultipleSpriteCheck(ISprite sprite)
        {
            if (mySprite is PlayerSprite && sprite is EnemySprite)
            {
                if (((EnemySprite)sprite).EnemyType == EnemySprite.EnemyTyping.BladeTrap)
                {
                    if (!(oldSprite is NullClass))
                    {
                        colliding = true;
                        oldSprite.Collision.IsColliding = true;
                    }
                }
            }

            else if (mySprite is PlayerSprite && sprite is ItemSprite)
            {
                
                    if (!(oldSprite is NullClass))
                    {
                        colliding = true;
                        oldSprite.Collision.IsColliding = true;
                    }
                
            }
        }

        //Checks every NEIGHBORING cell, but that name got long
        //Checks every cell that neighbors a cell in the object's current position.
        public Collection<ISprite> CheckEveryCell()
        {
            Collection<ISprite> adjacent = new Collection<ISprite>();
            foreach (Vector2 cell in gridPosition)
            {
                CheckNeighboringCell(cell, adjacent);
            }
            return adjacent;
        }

        //Checks every sprite in a neighboring cell
        public void GetIntercepts()
        {
            Collection<ISprite> adjSprites = CheckEveryCell();
            foreach (ISprite sprite in adjSprites)
            {
                if (!sprite.Equals(mySprite)) //Don't need to check collision against self
                {                    
                    CheckForIntercepts(sprite);
                }
            }

            if (!colliding)
            {
                collidingSprite = new NullClass();
            }
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        //Going to return an array of two elements, in case sprites collide diagonally. None occurs when
        //Sprites only collide in one direction.
        public enum CollisionDirection
        {
            Left, Right, Top, Bottom, BoundaryTop, BoundaryBottom, BoundaryLeft, BoundaryRight, None
        }

        //Assigns what direction the collision occured in between the two sprites
        //This takes care of all 9 directions and simplifies it to 5 by forcing diagonal directions to be either
        //vertical or horizontal collisions - Doesn't mean sprite can't move diagonally or that it won't pick up
        //diagonal collisions, just that both sprites react as if the collision is either vertical or horizontal

        public static void GetDirection(ISprite sprite, ISprite second)
        {
            //allowance for mario to move smoothly on floor
            const int allowance = 2;
            Rectangle overlap = Rectangle.Intersect(sprite.Collision.CollisionBox, second.Collision.CollisionBox);
            sprite.CollisionDirection = CollisionDirection.None;
            //Collide from top/bottom
            if (overlap.Width > overlap.Height)
            {

                if (overlap.Bottom > second.Collision.CollisionBox.Top && overlap.Bottom < second.Collision.CollisionBox.Bottom)
                {
                    sprite.CollisionDirection = CollisionDirection.Top;
                }
                else if (overlap.Top < second.Collision.CollisionBox.Bottom && overlap.Top > second.Collision.CollisionBox.Top)
                {
                    sprite.CollisionDirection = CollisionDirection.Bottom;
                }
            }
            else
            {                
                    if (overlap.Right > second.Collision.CollisionBox.Left + allowance &&
                        overlap.Right < second.Collision.CollisionBox.Right - allowance)
                    {
                        sprite.CollisionDirection = CollisionDirection.Left;
                    }
                    else if (overlap.Left < second.Collision.CollisionBox.Right - allowance &&
                             overlap.Left > second.Collision.CollisionBox.Left + allowance)
                    {
                        sprite.CollisionDirection = CollisionDirection.Right;
                    }
                
            }  
        }

       //If an object is inside another object
        private void MultipleSpriteSave(ISprite collided)
        {            
            if (collided is BlockSprite)
            {
                if (((BlockSprite)collided).BlockType == BlockSprite.Type.UnlockedDoor)
                {
                    oldSprite = collided;
                }
            }
        }

        //Detects X edge of screen
        private void GetBoundaryDirectionX(ISprite sprite)
        {
            if (boundaryX)
            {
                if (sprite.MovingPosition.X <= sprite.MyTexture.Width + 10)
                {
                    sprite.CollisionDirection = CollisionDirection.BoundaryLeft;
                }
                else
                {
                    sprite.CollisionDirection = CollisionDirection.BoundaryRight;
                }
            }
        }

        //Detects Y edge of screen
        private void GetBoundaryDirectionY(ISprite sprite)
        {
            if (boundaryY)
            {
                if (sprite.MovingPosition.Y <= sprite.MyTexture.Height + 10)
                {
                    sprite.CollisionDirection = CollisionDirection.BoundaryTop;
                }
                else
                {
                    sprite.CollisionDirection = CollisionDirection.BoundaryBottom;
                }
            }
        }

        //Need a way to pass the colliding sprite to mySprite
        public ISprite GetCollidingSprite
        {
            get { return collidingSprite; }
        }

        public void CheckHeightBoundary(ISprite sprite)
        {
          if (sprite.MovingPosition.Y < 0 || sprite.MovingPosition.Y 
              > _graphics.GraphicsDevice.Viewport.Height - 34) // Gives a cushion for super mario
          {
              boundaryY = true;
              GetBoundaryDirectionY(mySprite);
              collidingSprite = new NullClass();
              mySprite.CollisionUpdate();
            }
          else
          {
              boundaryY = false;
          }
        }

        public void CheckWidthBoundary(ISprite sprite)
        {
          if (sprite.MovingPosition.X < 0 || sprite.MovingPosition.X
                > grid.TotalWidth - 16)
            {
                boundaryX = true;
                GetBoundaryDirectionX(mySprite);
                collidingSprite = new NullClass();
                mySprite.CollisionUpdate();
            }
            else
            {
                boundaryX = false;
            }
        }
        //Checks if sprites should collide if velocity is added to movement

        //Just checks if one sprite is intercepting with mySprite
        public void CheckForIntercepts(ISprite sprite)
        {            
            //Sets current sprite's bounding box and the colliding sprite's bounding box to true if colliding
            if (boundingBox.Intersects(sprite.Collision.boundingBox)
                    && (sprite.Visible))
                {                    
                    colliding = true;
                    sprite.Collision.IsColliding = true;
                    GetDirection(mySprite, sprite);
                    GetDirection(sprite, mySprite);
                    if (!(collidingSprite is PlayerSprite)) //PlayerSprite is always highest priority
                    {                        
                        collidingSprite = sprite;
                    }
                    
                MultipleSpriteSave(collidingSprite);
                mySprite.CollisionUpdate();
            }
                else
                {
                    colliding = false;
                    sprite.Collision.IsColliding = false;
                    MultipleSpriteCheck(sprite);
                }            
        }

        //Checks if sprite's position has changed and removes sprite from old cells in grid
        public void RemoveOldCells()
        {
            foreach (Vector2 old in oldPosition)
            {
                if (!gridPosition.Contains(old))
                {
                    grid.RemoveSpriteFromCell(old, mySprite);
                }
            }                          
        }
        //Draws an outline of the bounding box, colored correctly.

        public void Draw(SpriteBatch spriteBatch)
        {
            const int thickness = 2;
            if (_visible && collisionOn)
            {
                DrawOutline(spriteBatch, thickness, color);                
            }
        }

        //Adds sprite to either moving or stationary list in collider.
        public void AddToMoving()
        {
            if (mySprite.Velocity.X > 0 || mySprite.Velocity.Y > 0)
            {
                collider.AddSpriteToMovingList(mySprite);
            }
            else
            {
                collider.AddSpriteToStationaryList(mySprite);
            }
        }

        //Determines if we need to add or subtract 1 for incrementing velocity
        //Example, if Mario is moving down (y=+1) or if mario is moving up (y=-1)
        //Returns a vector to ADD to current movement/SUBTRACT from current velocity
        public Vector2 ToAdd()
        {
            float x = 1;
            float y = 1;

            if (mySprite.Velocity.X < 0)
            {
                x = -1;
            }
            else if (mySprite.Velocity.X == 0)
            {
                x = 0;
            }
            if (mySprite.Velocity.Y < 0)
            {
                y = -1;
            }
            else if (mySprite.Velocity.Y == 0)
            {
                y = 0;
            }
            return new Vector2(x, y);
        }

        /* Need to constantly update size of bounding box (In case sprite animates/changes/moves) if sprite is moving, 
           Need to update object's current position if sprite is moving.
           Need to keep checking for intercepts of every neighboring cell
        */
        public void Update()
        {
            //This ensures tunneling doesn't occur by moving sprite incrementally and checking for collisions            
            do
            {
                if (collisionOn)
                {                       
                    if (mySprite.Velocity != Vector2.Zero)
                    {
                        DetermineBoxSize();
                        DetermineGridPosition();
                        RemoveOldCells();
                    }                   
                    GetIntercepts();                   
                }
                //Increments Position by 1 and then decrements Velocity by 1
                mySprite.MovingPosition += ToAdd();
                mySprite.SetVelocity(mySprite.Velocity - ToAdd());
            }
            while (mySprite.Velocity != Vector2.Zero) ;   
        }

        public void TurnOffCollision()
        {
            collisionOn = false;
            DetermineBoxSize();
            DetermineGridPosition();
            foreach (Vector2 position in oldPosition)
            {
                grid.RemoveSpriteFromCell(position, mySprite);
            }

        }

        public void TurnOnCollision()
        {
            collisionOn = true;
            DetermineBoxSize();
            DetermineGridPosition();
            foreach (Vector2 position in oldPosition)
            {
                grid.AddSpriteToCell(position, mySprite);
            }
        }
    }
}

