using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.MovementStates
{
    public class WalkingState : IMovementState
    {
        private readonly MarioMoveState myMario;

        public WalkingState(MarioMoveState mario)
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

     
    
        public void ToWalking()
        {
            //Already here
        }

        static public void ToWalkingDown()
        {
           
        }
        static public void ToAttack()
        {

        }

        public void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics)
        {
            if (mario != null)
            {
                switch (command)
                {
                    case MarioMoveState.Command.Up:
                        mario.PreviousMoveState = mario.MoveState.Walking;
                        mario.SetVelocity(new Vector2(0, -2));                    
                        break;
                    case MarioMoveState.Command.UpRelease:
                    case MarioMoveState.Command.DownRelease:
                        ToIdle();
                        mario.Move(graphics, PlayerSprite.MovementDirection.Stop);
                        break;

                    case MarioMoveState.Command.Down:
                        mario.PreviousMoveState = mario.MoveState.Walking;
                        ToWalkingDown();
                        mario.SetVelocity(new Vector2(0, 2));
                        break;

                    case MarioMoveState.Command.Left: //Left
                        break;

                    case MarioMoveState.Command.Right: //Right
                        break;

                    case MarioMoveState.Command.Stop:
                        ToIdle();
                        mario.Move(graphics, PlayerSprite.MovementDirection.StopX);
                        break;
                }
            }
        }

        public void Action(PlayerSprite mario)
        {
            if (mario != null)
            {
                switch (mario.MyDirection)
                {
                    case PlayerSprite.Direction.Up:
                        mario.Texture = mario.GetWalkUpTexture;
                        break;
                    case PlayerSprite.Direction.Down:
                        mario.Texture = mario.GetWalkDownTexture;
                        break;
                    case PlayerSprite.Direction.Right:
                    case PlayerSprite.Direction.Left:
                        mario.Texture = mario.GetWalkRightTexture;
                        break;
                }
                    mario.CurrentFrame++;
                    if(mario.CurrentFrame > 1)
                    {
                        mario.CurrentFrame = 0;
                    }
                
            }
        }
    }
}