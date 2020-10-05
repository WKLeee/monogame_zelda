using Sprint_1.Block;
using Sprint_1.Sprites;

namespace Sprint_1.Interfaces
{
    public interface IBlockState
    {
        void ToQuestion();
        void ToUsed();
        void ToBrick();
        void ToHidden();
        void ToBump();
        void ToDebris();
        void Execute(BlockState.Command command, BlockSprite block, PlayerSprite sprite);
        void Action(BlockSprite block);
    }
}
