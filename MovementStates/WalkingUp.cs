using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.MovementStates
{
    public class WalkingUp : IMovementState
    {
        private readonly MarioMoveState myMario;       

        public WalkingUp(MarioMoveState mario)
        {
            myMario = mario;
        }       
        
        public void ToIdle()
        {
            myMario.SetState(myMario.Idle);
        }

         public void ToJump()
        {
            myMario.SetState(myMario.Jump);
        }
    
        public void ToFall()
        {
            
            myMario.SetState(myMario.Fall);
        }

        public void ToRunning()
        {
            myMario.SetState(myMario.Running);
        }

        public void ToWalking()
        {
        }

        public void ToCrouch()
        {
            myMario.SetState(myMario.Crouch);
        }

        

        public void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics)
        {
            if (mario != null)
            {
                mario.Jumping = true;
                switch (command)
                {
                    case MarioMoveState.Command.Down:                       
                        ToIdle();
                        break;
                    case MarioMoveState.Command.Left:
                        if (mario.IsFlipped)
                        {
                            mario.Move(graphics, PlayerSprite.MovementDirection.LeftHold);
                        }
                        else
                        {
                            mario.Flip();
                        }
                        break;
                    case MarioMoveState.Command.Right:
                        if (!mario.IsFlipped)
                        {
                            mario.Move(graphics, PlayerSprite.MovementDirection.RightHold);
                        }
                        else
                        {
                            mario.Flip();
                        }
                        break;
                    case MarioMoveState.Command.Up:
                        break;

                    case MarioMoveState.Command.UpRelease:
                        mario.Move(graphics, PlayerSprite.MovementDirection.Stop);
                        break;

                    case MarioMoveState.Command.Stop:
                        mario.Move(graphics, PlayerSprite.MovementDirection.StopX);
                        break;
                }
            }
        }

        public void Action(PlayerSprite mario)
        {
            mario.Texture = mario.GetWalkUpTexture;
        }
    }
}