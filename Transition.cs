using Microsoft.Xna.Framework;
using Sprint_1.Sprites;

namespace Sprint_1
{

    //Used so playerSprite can communicate to Game1 when player goes to different room
    //May possible expand later
    public class Transition
    {
        public Vector2 Adjustment { get; set; }
        private bool change;
        public BlockSprite.DoorDirection Direction { get; set; }
        public int VerticalFix { get; set; }
        public BlockSprite Door { get; set; }
        public int RoomSize { get; set; }
        private int count;
        public Camera Camera { get; set; }
        public Transition(Camera c)
        {
            Camera = c;
            RoomSize = 384;
            Direction = BlockSprite.DoorDirection.NA;
            Adjustment = Vector2.Zero;
            change = false;
            count = 0;
        }

        public void Receive(bool update)
        {
            change = update;
            if (update)
            {
                count++;
            }
            else
            {
                count = 0;
            }
        }

        //There is only 1 row of vertical blocks between rooms. Each room needs to fill the whole screen width (800 px)
        //1 block = 16px. Therefore in room 1, vertical blocks at edge of room are at 0 and 784. Next room starts at 784
        //and far wall starts 784 away, at 1568. Add 16 for the width of block = 1584. Which is not 1600. This trend continues 
        //on the further right you go. Therefore, this fixes the issue whenever camera needs to change rooms.
        public int RoomBoundaryFix()
        {
            
            int roomCount = (int)Door.MovingPosition.X / RoomSize;
            if (roomCount > 0)
            {
                roomCount = 1;
            }
            return roomCount;
        }

        public bool Check()
        {
            return change;
        }

        public int Count
        {
            get { return count; }
            set { count = value;  }
        }

        public void DoorSwitch()
        {
            Door.Door = Direction;
            if (Door.Door == BlockSprite.DoorDirection.Top || Door.Door == BlockSprite.DoorDirection.Bottom)
            {
                Door.MovingPosition += new Vector2(0, VerticalFix);
                Door.Collision.DetermineBoxSize();
                Door.Collision.DetermineGridPosition();
            }
        }
    }
}
