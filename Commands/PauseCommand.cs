using System;
using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class PauseCommand : ICommand
    {
        private readonly Game1 myGame;
        private readonly String pause = "Pause";

        public PauseCommand(Game1 game)
        {
            myGame = game;
        }

        public void Execute()
        {
           CommandHandler.Execute(myGame, pause);
        }
    }
}
