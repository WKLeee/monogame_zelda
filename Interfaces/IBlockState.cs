
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
        void Action(BlockSprite block);
    }
}
