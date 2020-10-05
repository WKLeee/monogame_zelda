using Sprint_1.Interfaces;

namespace Sprint_1.Commands
{
    class PrintChordCommand : ICommand
    {
        private readonly string myPressed;
        private readonly string Message = "You pressed the chord: ";

        public PrintChordCommand(string pressed)
        {
            myPressed = pressed;
        }

        public void Execute()
        {
            CommandHandler.Execute(Message, myPressed);
        }
    }
}
