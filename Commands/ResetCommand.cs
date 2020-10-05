using System;
using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class ResetCommand : ICommand
    {
        private readonly Game1 myGame;
        private readonly String reset = "Reset";

        public ResetCommand(Game1 game)
        {
            myGame = game;
        }

        public void Execute()
        {
            CommandHandler.Execute(myGame, reset);
        }       
    }
} 