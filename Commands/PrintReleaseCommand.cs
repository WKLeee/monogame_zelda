using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class PrintReleaseCommand : ICommand
    {
        private readonly string myKey;
        private readonly string Message = "You have released: ";

        public PrintReleaseCommand(string key)
        {
            myKey = key;
        }

        public void Execute()
        {
            CommandHandler.Execute(Message, myKey);
        }
    }
}
