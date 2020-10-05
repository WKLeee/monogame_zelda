using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;

namespace Sprint_1
{
    public class Collider
    {
        private Grid grid;
        private Dictionary<ISprite, Collection<Vector2>> nextCells; //Keeps track of the sprite's current cells
        private Collection<ISprite> collisionList; //If moving sprites will collide
        internal Collection<ISprite> movingSprites; //All sprites that move (Sprites added in their collision class)
        private Collection<ISprite> stationarySprites;//All sprites that don't move (Sprites added in collision class)

        public Collider(Grid board)
        {
            nextCells = new Dictionary<ISprite, Collection<Vector2>>();
            movingSprites = new Collection<ISprite>();
            grid = board;
            stationarySprites = new Collection<ISprite>();
            collisionList = new Collection<ISprite>();
        } 

        //Adds a moving sprite to the moving list
        public void AddSpriteToMovingList(ISprite sprite)
        {
            movingSprites.Add(sprite);
        }

        public void AddSpriteToStationaryList(ISprite sprite)
        {
            stationarySprites.Add(sprite);
        }


        //Adds all sprites in neighboring cell to adjacent list
        public void AddToNeighborList(Vector2 cell, Collection<ISprite> adj)
        {
            Collection<ISprite> spritesInCell = grid.GetSpritesInCell(cell);
            //Adds every sprite in the grid's list for cell
            foreach (ISprite sprite in spritesInCell)
            {
                if (sprite.Visible)
                {
                    adj.Add(sprite);
                }
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
                int plus = (int)cell.X + 1;
                cellForward = new Vector2(plus, cell.Y);
                AddToNeighborList(cellForward, adjacent); //Adds to list if cell isn't the same cell as one being checked
            }

            if ((int)cell.Y != 0)
            {
                int down = (int)cell.Y - 1;
                cellDown = new Vector2(cell.X, down);
                AddToNeighborList(cellDown, adjacent); //Adds to list if cell isn't the same cell as one being checked
            }

            if ((int)cell.Y != grid.HeightInCells)
            {
                int up = (int)cell.Y + 1;
                cellUp = new Vector2(cell.X, up);
                AddToNeighborList(cellUp, adjacent); //Adds to list if cell isn't the same cell as one being checked
            }
        }
        //Checks every cell that neighbors a cell in the object's current position.
        public Collection<ISprite> CheckEveryCell(ISprite current)
        {
            Collection<ISprite> adjacent = new Collection<ISprite>();
            foreach (Vector2 cell in nextCells[current])
            {
                CheckNeighboringCell(cell, adjacent);
            }
            return adjacent;
        }

        //Just checks if one is intercepting
        // ReSharper disable once InconsistentNaming
        public void CheckForIntercepts(ISprite sprite, ISprite mySprite)
        {          
                //Sets current sprite's bounding box and the colliding sprite's bounding box to true if colliding
                if (sprite.Collision.CollisionBox.Intersects(mySprite.Collision.CollisionBox) 
                    && !collisionList.Contains(mySprite))
                {
                    if (sprite.Visible)
                {
                        collisionList.Add(mySprite);
                    }
                }                                       
        }

        //Checks every sprite in a neighboring cell
        public void GetIntercepts(ISprite currentSprite)
        {
            Collection<ISprite> adjSprites = CheckEveryCell(currentSprite);
            
            foreach (ISprite sprite in adjSprites)
            {
                if (!sprite.Equals(currentSprite)) //Don't need to check collision against self
                {
                    CheckForIntercepts(sprite, currentSprite);
                }
            }
        }

        //Gets the fastest velocity from a sprite, either X or Y
        private static double FastestSpeed(ISprite sprite)
        {
            double fast = sprite.Velocity.X;
            if (sprite.Velocity.Y > fast)
            {
                fast = sprite.Velocity.Y;
            }
            return fast;
        }
        //Returns true if first sprite arg is faster than second sprite arg
        private static bool FastestTime(ISprite fast, ISprite sprite)
        {
            double fastSpeed = FastestSpeed(fast);
            double spriteSpeed = FastestSpeed(sprite);
            return fastSpeed < spriteSpeed;
        }

        //Used to sort collisions, earliest first. Bubble sort
        public void SortList()
        {
            for (int i = 0; i < collisionList.Count - 1; i++)
            {
                for (int j = i + 1; j < collisionList.Count; j++)
                {
                    //If j is faster than i, swap them
                    if (FastestTime(collisionList[j], collisionList[i]))
                    {
                        ISprite temp = collisionList[i];
                        collisionList[i] = collisionList[j];
                        collisionList[j] = temp;
                    }
                }
            }
        }        

        //Adds each proposed cell to the gridList
        private static void AddToGrid(Vector2 vector, Collection<Vector2> gridPosition)
        {
            if (!gridPosition.Contains(vector)) //In case the object is in less than 4 cells
            {
                gridPosition.Add(vector);
            }           
        }

        //Sets up the vectors as appropriate cells
        //Method in CollisionObject takes care of getting direction of collision - ensures only 4 cells returned
        //Yet diagonal movement works - it determines if mario comes from the side or top, as only one direction 
        //is only ever actually used (IE, coming from top left to a goomba will either kill mario or kill gomba - never both)
        //Therefore, the collisions are still properly setup for diagonal movement, but are somewhat simplified.
        private static void GetFutureCells(int width, int height, int x, int y, Collection<Vector2> gridPosition)
        {
            Vector2 block1 = new Vector2(width, height);
            Vector2 block2 = new Vector2(width, y);
            Vector2 block3 = new Vector2(x, height);
            Vector2 block4 = new Vector2(x, y);

            //A single cell could have up to 4 neighboring cells (Less if a sprite spans more than one cell)
            AddToGrid(block1, gridPosition);
            AddToGrid(block2, gridPosition);
            AddToGrid(block3, gridPosition);
            AddToGrid(block4, gridPosition);
        }

        //Gets what cell(s) each moving sprite will be in if velocity is added
        public void CalculateNextCell()
        {
            foreach (ISprite sprite in movingSprites)
            {               
                Collection<Vector2> gridPosition = new Collection<Vector2>(); //Creates a new list each time so we don't have to remove objects
                double[] coords = sprite.FutureGridPosition();
                int widthInt = CollisionObject.ConvertToInt(coords[0]);
                int heightInt = CollisionObject.ConvertToInt(coords[1]);
                int locationXInt = CollisionObject.ConvertToInt(coords[2]);
                int locationYInt = CollisionObject.ConvertToInt(coords[3]);
                GetFutureCells(locationXInt, locationYInt, widthInt, heightInt, gridPosition); //Adds these cells to current position list
                nextCells.Add(sprite, gridPosition);
                GetIntercepts(sprite);
            }
        }

        public void VelocityUpdate(Vector2 velocityToAdd, int index)
        {
            ISprite sprite = collisionList[index];
            foreach (ISprite updateMove in collisionList)
            {
                //Checks for negatives
                Vector2 trueAdd = UpdatePosition(sprite, velocityToAdd);
                double x = sprite.Velocity.X - trueAdd.X;
                double y = sprite.Velocity.Y - trueAdd.Y;
                updateMove.SetVelocity(new Vector2((float)x, (float)y));
            }
            collisionList.RemoveAt(index);
        }

        //Need to make sure we are adding/subtracting Position/Velocity when we should be
        public static Vector2 UpdatePosition(ISprite sprite, Vector2 toAdd)
        {
            double xToAdd, yToAdd;
            if (sprite.MovingPosition.X > 0 && toAdd.X > 0)
            {
                xToAdd = toAdd.X;
            }
            else if (sprite.MovingPosition.X < 0 && toAdd.X > 0)
            {
                xToAdd = -toAdd.X;
            }
            else if (sprite.MovingPosition.X > 0 && toAdd.X < 0)
            {
                xToAdd = -toAdd.X;
            }
            else
            {
                xToAdd = toAdd.X;
            }

            if (sprite.MovingPosition.Y > 0 && toAdd.Y > 0)
            {
                yToAdd = toAdd.Y;
            }
            else if (sprite.MovingPosition.Y < 0 && toAdd.Y > 0)
            {
                yToAdd = -toAdd.Y;
            }
            else if (sprite.MovingPosition.Y > 0 && toAdd.Y < 0)
            {
                yToAdd = -toAdd.Y;
            }
            else
            {
                yToAdd = toAdd.Y;
            }
            Vector2 vectorToAdd = new Vector2((float)xToAdd, (float)yToAdd);
            return vectorToAdd;
        }


        //Updates all sprite's Position based off of a single sprite's velocity
        public Vector2 Move(int index)
        {
            ISprite sprite = collisionList[index];            
            Vector2 velocityToAdd = sprite.Velocity;
            foreach (ISprite updateMove in collisionList)
            {
                //Checks for negatives
                Vector2 trueAdd = UpdateVelocity(sprite, velocityToAdd);
                updateMove.MovingPosition += trueAdd;
            }
            return velocityToAdd;
        }

        //Need to make sure we are adding/subtracting Position/Velocity when we should be
        public static Vector2 UpdateVelocity(ISprite sprite, Vector2 toAdd)
        {
            double xToAdd, yToAdd;
            if (sprite.Velocity.X > 0 && toAdd.X > 0)
            {
                xToAdd = toAdd.X;
            }
            else if (sprite.Velocity.X < 0 && toAdd.X > 0)
            {
                xToAdd = -toAdd.X;
            }
            else if (sprite.Velocity.X > 0 && toAdd.X < 0)
            {
                xToAdd = -toAdd.X;
            }
            else
            {
                xToAdd = toAdd.X;
            }

            if (sprite.Velocity.Y > 0 && toAdd.Y > 0)
            {
                yToAdd = toAdd.Y;
            }
            else if (sprite.Velocity.Y < 0 && toAdd.Y > 0)
            {
                yToAdd = -toAdd.Y;
            }
            else if (sprite.Velocity.Y > 0 && toAdd.Y < 0)
            {
                yToAdd = -toAdd.Y;
            }
            else
            {
                yToAdd = toAdd.Y;
            }
            Vector2 vectorToAdd = new Vector2((float)xToAdd, (float)yToAdd);
            return vectorToAdd;
        }

        //The actual update function in sprite class needs to run last
        public void UpdateRest(GameTime gameTime)
        {
            foreach (ISprite moving in movingSprites)
            {
                moving.Update(gameTime);
            }

            ISprite[] tempStationary = stationarySprites.ToArray();
            foreach (ISprite stationary in tempStationary)
            {
                stationary.Update(gameTime);
                
            }
        }

        public void Update(GameTime gameTime)
        {
            int index = 0;
            do
            {               
                CalculateNextCell();
                SortList();

                //Checks through each colliding sprite. Multiple sprites in 1 cell are supported, that logic is found in
                //collisionObject, in methods AddToNeighborList and CheckNeighboringCell
                if (collisionList.Count > 0) 
                {
                    Vector2 velocity = Move(index);
                    VelocityUpdate(velocity, index);
                    index++;
                }
            } while (collisionList.Count > 0);
            UpdateRest(gameTime);            
        }
    }
}
