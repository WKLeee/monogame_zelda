using Microsoft.Xna.Framework;
using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class QuitCommand : ICommand
    {
        private readonly Game myGame;
        private readonly string quit = "Quit";

        public QuitCommand(Game game)
        {
            myGame = game;
        }

        public void Execute()
        {
            CommandHandler.Execute(myGame, quit);
        }
    }
}
