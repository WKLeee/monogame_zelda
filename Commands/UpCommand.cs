using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.MovementStates;
using Sprint_1.Sprites;

namespace Sprint_1.Commands
{
    class UpCommand : ICommand
    {
        private readonly PlayerSprite mySprite;
        private readonly MarioMoveState.Command command = MarioMoveState.Command.Up;
        private readonly GraphicsDeviceManager graphics;
        public UpCommand(PlayerSprite sprite, GraphicsDeviceManager device)
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