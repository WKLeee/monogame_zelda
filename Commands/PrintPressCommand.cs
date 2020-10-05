using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class PrintPressCommand : ICommand
    {
        private readonly string myKey;
        private readonly string Message = "You pressed: ";

        public PrintPressCommand(string key)
        {
            myKey = key;
        }

        public void Execute()
        {
            CommandHandler.Execute(Message, myKey);
        }
    }
}
