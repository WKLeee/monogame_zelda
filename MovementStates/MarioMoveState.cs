using System.Diagnostics.CodeAnalysis;
using Sprint_1.Interfaces;

namespace Sprint_1.MovementStates
{
    //Context class for state
    public class MarioMoveState
    {
        public IMovementState Idle { get; set; }
        public IMovementState Crouch { get; set; }
        public IMovementState Jump { get; set; }
        public IMovementState Walking { get; set; }
        public IMovementState Running { get; set; }
        public IMovementState Fall { get; set; }
        public IMovementState CannotMove { get; set; }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public enum Command {Up, UpRelease, Down, DownRelease, Left, Right, Stop, RightHold, LeftHold };
            
        private IMovementState current;

        public IMovementState Current
        {
            get { return current;}
            set { current = value; }
        }


        public MarioMoveState()
            {
                Idle = new IdleState(this);
                Crouch = new WalkingDown(this);
                Jump = new WalkingUp(this);
                Walking = new WalkingState(this);
                Fall = new FallState(this);
                CannotMove = new CannotMoveState();
                current = Idle; //Idle is default
                //myNum = ;
            }

        public void SetState(IMovementState state) //Setter this way for clarity of what is happening
        {
            current = state;
        }
    }
}