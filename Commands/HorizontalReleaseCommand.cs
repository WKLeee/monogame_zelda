using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.MovementStates;
using Sprint_1.Sprites;

namespace Sprint_1.Commands
{
    class HorizontalReleaseCommand : ICommand
    {
        private readonly PlayerSprite mySprite;
        private readonly MarioMoveState.Command command = MarioMoveState.Command.Stop;
        private readonly GraphicsDeviceManager graphics;

        public HorizontalReleaseCommand(PlayerSprite sprite, GraphicsDeviceManager device)
        {
            graphics = device;
            mySprite = sprite;
        }

        public void Execute()
        {
            CommandHandler.Execute(command, mySprite, graphics);

        }
    }
}
