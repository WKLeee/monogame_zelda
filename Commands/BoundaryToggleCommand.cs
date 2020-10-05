using Microsoft.Xna.Framework.Graphics;
using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class BoundaryToggleCommand : ICommand
    {
        private readonly Texture2D myPixel;

        public BoundaryToggleCommand(Texture2D pixel)
        {
            myPixel = pixel;
        }

        public void Execute()
        {
            CommandHandler.Execute(myPixel);
        }
    }
}