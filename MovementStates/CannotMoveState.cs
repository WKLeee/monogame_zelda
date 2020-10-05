using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.MovementStates
{
    public class CannotMoveState : IMovementState
    {

         public void ToIdle()
        {
            //Cant move cause the player is dead
        }

         public void ToFall()
        {
            //Already here
        }

         static public void ToWalkingUp()
        {
            //Can't reach from here.
        }

         public void ToWalking()
        {
            //Can't reach from here.
        }

        static public void ToWalkingDown()
        {
            //Can't reach
        }

        static public void ToAttack()
        {
            //Can't reach
        }
        public void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics)
        { 
            //Cannot do anything
        }

        public void Action(PlayerSprite mario)
        {

        }
    }
}