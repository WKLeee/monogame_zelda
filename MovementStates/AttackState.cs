using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.MovementStates
{
    public class AttackState : IMovementState
    {
        private readonly MarioMoveState myMario;

        public AttackState(MarioMoveState mario)
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

        public void ToWalkingUp()
        { 
            myMario.SetState(myMario.Jump);
        }
    
        public void ToWalking()
        {
            myMario.SetState(myMario.Walking);
        }

        public void ToWalkingDown()
        {
            myMario.SetState(myMario.WalkingDown);
        }
        public void ToAttack()
        {
            //Already here
        }
        public void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics)
        {
            if (mario != null)
            {
                switch (command)
                {
                    case MarioMoveState.Command.Up:
                        mario.PreviousMoveState = mario.MoveState.Walking;

                        if (mario.Velocity.Y > 0)
                        {
                            mario.Move(graphics, PlayerSprite.MovementDirection.Stop);
                        }
                        else
                        {
                            ToWalkingUp();
                            mario.Move(graphics, PlayerSprite.MovementDirection.UpHold);
                        }
                        break;

                    case MarioMoveState.Command.Down:
                        ToWalkingDown();
                        mario.SetVelocity(new Vector2(mario.Velocity.X, 2));
                        break;

                    case MarioMoveState.Command.Left: //Left
                        if (!mario.IsFlipped) //walking right
                        {
                            ToIdle();
                            if (mario.MovingPosition.Y < 448)                                
                            {
                                mario.Move(graphics, PlayerSprite.MovementDirection.DownHold);
                            }
                        }
                        else
                        {
                            mario.Move(graphics, PlayerSprite.MovementDirection.LeftHold);

                        }

                        break;
                    case MarioMoveState.Command.Right: //Right
                        if (mario.IsFlipped) //Walking left
                        {
                            ToIdle();
                        }
                        else
                        {       
                            mario.Move(graphics, PlayerSprite.MovementDirection.RightHold);                          

                        }

                        break;

                    case MarioMoveState.Command.RightHold:
                        if (!mario.IsFlipped)
                        {
  
                            mario.Move(graphics, PlayerSprite.MovementDirection.RightHold);
                        }

                        break;
                    case MarioMoveState.Command.LeftHold:
                        if (mario.IsFlipped)
                        {
  
                            mario.Move(graphics, PlayerSprite.MovementDirection.LeftHold);
                        }

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
                if(mario.gotSword)
                {
                    mario.Texture = mario.GetWeaponRightTexture;
                }
                else mario.Texture = mario.GetWalkRightTexture;

                if (mario.CanCrouch) //Super mario walking frames are 2-4
                {
                    mario.CurrentFrame++;
                    if (mario.CurrentFrame > 9)
                    {
                        mario.CurrentFrame = 2;
                    }
                }
                else
                {
                    //Standard Mario walking frames are 1-3
                    mario.CurrentFrame++;
                    if (mario.CurrentFrame > 9)
                    {
                        mario.CurrentFrame = 1;
                    }
                }
            }
        }
    }
}