using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.Commands
{
    class AttackReleaseCommand : ICommand
    {
        private readonly PlayerSprite mySprite;

        public AttackReleaseCommand(PlayerSprite sprite)
        {
            mySprite = sprite;
        }

        public void Execute()
        {
            mySprite.AttackRelease();
        }
    }
}
