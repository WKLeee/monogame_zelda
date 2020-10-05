using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.MovementStates;
using Sprint_1.Sprites;

namespace Sprint_1.Commands
{
    class LeftCommand : ICommand
    {
        private readonly PlayerSprite mySprite;
        private readonly MarioMoveState.Command command = MarioMoveState.Command.Left;
        private readonly GraphicsDeviceManager graphics;

        public LeftCommand(PlayerSprite sprite, GraphicsDeviceManager device)
        {
            mySprite = sprite;
            graphics = device;
        }

        public void Execute()
        {
            CommandHandler.Execute(command, mySprite, graphics);
        }
    }
}