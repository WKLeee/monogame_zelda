using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.Commands
{
    class UpHoldCommand : ICommand
    {
        private readonly ISprite mySprite;
        private readonly GraphicsDeviceManager graphics;

        public UpHoldCommand(ISprite sprite, GraphicsDeviceManager device)
        {
            graphics = device;
            mySprite = sprite;
        }

        public void Execute()
        {
            CommandHandler.Execute((PlayerSprite)mySprite, graphics, PlayerSprite.MovementDirection.UpHold);
        }
    }
}
