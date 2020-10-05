using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class PrintHoldCommand : ICommand
    {
        private readonly string myKey;
        private readonly string Message = "You are holding: ";

        public PrintHoldCommand(string key)
        {
            myKey = key;
        }

        public void Execute()
        {
            CommandHandler.Execute(Message, myKey);
        }
    }
}
