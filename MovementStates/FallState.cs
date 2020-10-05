using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.MovementStates
{
    public class FallState : IMovementState
    {
        private readonly MarioMoveState myMario;

        public FallState(MarioMoveState mario)
        {
            myMario = mario;
        }

        public void ToIdle()
        {
            myMario.SetState(myMario.Idle);
        }

        public void ToWalkingUp()
        {
            myMario.SetState(myMario.Jump);
        }

        public void ToFall()
        {
           //Here
        }

        public void ToWalking()
        {
            myMario.SetState(myMario.Walking);
        }

        public void ToWalkingDown()
        {
            myMario.SetState(myMario.Crouch);
        }
        static public void ToAttack()
        {

        }
        public void ToPrevious(IMovementState previous)
        {
            if (previous is WalkingState)
            {
                ToWalking();
            }
            else
            {
                ToIdle();
            }
        }

        public void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics)
        {
            
            if (mario != null)
            {
                switch (command)
                {
                    case MarioMoveState.Command.Up:
                        
                        break;
                    case MarioMoveState.Command.Down:
                        if (!mario.Jumping)
                        {
                            if (mario.CanCrouch)
                            {
                                ToWalkingDown();
                            }
                            else
                            {
                                ToIdle();
                                if (graphics != null)
                                {
                                    if (mario.MovingPosition.Y < graphics.GraphicsDevice.Viewport.Height -
                                        mario.Texture.Height - 5)
                                    {
                                        mario.Move(graphics, PlayerSprite.MovementDirection.DownHold);
                                    }
                                }
                            }

                            mario.Move(graphics, PlayerSprite.MovementDirection.DownHold);

                        }
                        else
                        {
                            mario.CurrentFrame = 0;
                            mario.SetVelocity(new Vector2(mario.Velocity.X, 1));
                            mario.Jumping = false;
                        }

                        break;
                    case MarioMoveState.Command.Left: //Left
                        if (!mario.IsFlipped) //If facing right
                        {
                            mario.Flip();
                        }
                        else
                        {
                            mario.CurrentFrame = 2; //Sets frame to proper spot for walking
                        }
                        mario.Move(graphics, PlayerSprite.MovementDirection.LeftHold);

                        break;
                    case MarioMoveState.Command.Right: //Right
                        if (mario.IsFlipped) //Facing left
                        {
                            mario.Flip();
                        }
                        else
                        {
                            mario.CurrentFrame = 2; //Sets frame to proper spot for walking
                        }
                        mario.Move(graphics, PlayerSprite.MovementDirection.RightHold);

                        break;
                    case MarioMoveState.Command.Stop:
                        mario.CurrentFrame = 0;
                        mario.Move(graphics, PlayerSprite.MovementDirection.StopX);
                        break;
                }
            }
        }
        public void Action(PlayerSprite mario)
        {
            if (mario != null)
            {
                mario.CurrentFrame = 0;
            }
        }
    }
}