using Sprint_1.PowerUpStates;

namespace Sprint_1.Interfaces
{
	public interface IPowerUpState
	{
        void ToSuper();
        void ToFire();
        void ToStar();
        void ToSmall();
        void ToDeath();
        void Execute(PowerUpStateAbstract.Command command, ISprite sprite);
        void Action(ISprite sprite);

	}
}
