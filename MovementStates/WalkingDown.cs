using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.MovementStates
{
    public class WalkingDown : IMovementState
    {
        private readonly MarioMoveState myMario;

        public WalkingDown(MarioMoveState mario)
        {
            myMario = mario;
        }
        
         public void ToIdle()
        {
            myMario.SetState(myMario.Idle);
        }

        public void ToFall()
        {
            myMario.SetState(myMario.Fall);
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
            //Already here
        }
        static public void ToAttack()
        {

        }

        public void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics) 
        {
            if (graphics != null && mario != null)
            {
                switch (command)
                {
                    case MarioMoveState.Command.Up:
                        ToWalkingUp();
                        mario.SetVelocity(new Vector2(0, -2));

                        break;

                    case MarioMoveState.Command.Down:

                        break;

                    case MarioMoveState.Command.DownRelease:
                        ToIdle();
                        mario.Move(graphics, PlayerSprite.MovementDirection.Stop);

                        break;

                    case MarioMoveState.Command.Left:
                        if (!mario.IsFlipped)
                        {
                            mario.Flip();
                        }

                        break;
                    case MarioMoveState.Command.Right:
                        if (mario.IsFlipped)
                        {
                            mario.Flip();
                        }

                        break;
                    
                }
            }
        }

        public void Action(PlayerSprite mario)
        {
            if (mario != null)
            {
                mario.Texture = mario.GetWalkDownTexture;
            }
        }
    }
}