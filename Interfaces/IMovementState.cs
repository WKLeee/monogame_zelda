using Microsoft.Xna.Framework;
using Sprint_1.MovementStates;
using Sprint_1.Sprites;

namespace Sprint_1.Interfaces
{
    public interface IMovementState
    {
        void ToWalking();
        void ToIdle();
        void ToFall();

        void Execute(MarioMoveState.Command command, PlayerSprite mario, GraphicsDeviceManager graphics); //Controls if sprite needs to switch state.
        void Action(PlayerSprite mario); //Action controls what sprite frame appears.
    }
}