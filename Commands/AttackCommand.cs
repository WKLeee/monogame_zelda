using Sprint_1.Interfaces;
using Sprint_1.Sprites;

namespace Sprint_1.Commands
{
    class AttackCommand : ICommand
    {
        private readonly PlayerSprite mySprite;

        public AttackCommand(PlayerSprite sprite)
        {
            mySprite = sprite;
        }

        public void Execute()
        {
            mySprite.Attack();
        }
    }
}
