using Microsoft.Xna.Framework;

namespace Sprint_1.Interfaces
{
    interface IController
    {
        void AddKeyMapping(string key, int action, ICommand command);

        void Update(GameTime gameTime);
    }
}
