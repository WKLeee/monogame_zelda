using System;
using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class MuteCommand : ICommand
    {
        private readonly Game1 myGame;
        private readonly String mute = "Mute";

        public MuteCommand(Game1 game)
        {
            myGame = game;
        }

        public void Execute()
        {
           CommandHandler.Execute(myGame, mute);
        }
    }
}
