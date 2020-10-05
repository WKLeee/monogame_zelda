using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class SpriteVisibilityToggleCommand : ICommand
    {
        private readonly ISprite mySprite;

        public SpriteVisibilityToggleCommand(ISprite sprite)
        {
            mySprite = sprite;
        }

        public void Execute()
        {
            CommandHandler.Execute(mySprite);
        }
    }
}
