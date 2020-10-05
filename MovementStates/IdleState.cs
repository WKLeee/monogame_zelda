using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.MovementStates
{
    public class IdleState : IMovementState
    {
        private readonly MarioMoveState myMario;
    
        public IdleState(MarioMoveState mario) {
            myMario = mario;
    }

         public void ToIdle()
        {
            //Already here
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
            myMario.SetState(myMario.Walking);
        }

         public void ToCrouch()
        {
            myMario.SetState(myMario.Crouch);
        }

        public void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics) 
        {
            if (mario != null)
            {
                switch (command)
                {
                    case MarioMoveState.Command.Up:
                        if (mario.MyDirection == PlayerSprite.Direction.Up)
                        {
                            ToWalking();
                            mario.Move(graphics, PlayerSprite.MovementDirection.UpHold);

                        }

                        mario.Texture = mario.GetWalkUpTexture;
                        mario.MyDirection = PlayerSprite.Direction.Up;
                        
                        break;
                    case MarioMoveState.Command.Down:
                        if (mario.MyDirection == PlayerSprite.Direction.Down)
                            {
                                ToWalking();
                                mario.Move(graphics, PlayerSprite.MovementDirection.DownHold);
                                
                            }
                        

                        mario.Texture = mario.GetWalkDownTexture;

                        mario.MyDirection = PlayerSprite.Direction.Down;

                        break;
                    case MarioMoveState.Command.Left: //Left
                        if (!mario.IsFlipped) //If facing right
                        {
                            mario.Flip();
                        }
                        else
                        {
                            mario.CurrentFrame = 2; //Sets frame to proper spot for walking
                            ToWalking();
                        }
                        mario.Move(graphics, PlayerSprite.MovementDirection.LeftHold);

                        mario.MyDirection = PlayerSprite.Direction.Left;


                        break;
                    case MarioMoveState.Command.LeftHold: //Left
                        if (!mario.IsFlipped) //If facing right
                        {
                            mario.Flip();
                        }
                        else
                        {
                            mario.CurrentFrame = 2; //Sets frame to proper spot for walking
                            ToWalking();
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
                            ToWalking();
                        }
                        mario.Move(graphics, PlayerSprite.MovementDirection.RightHold);

                        mario.MyDirection = PlayerSprite.Direction.Right;


                        break;
                    case MarioMoveState.Command.RightHold: //Right
                        if (mario.IsFlipped) //Facing left
                        {
                            mario.Flip();
                        }
                        else
                        {
                            mario.CurrentFrame = 2; //Sets frame to proper spot for walking
                            ToWalking();
                        }
                        mario.Move(graphics, PlayerSprite.MovementDirection.RightHold);

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