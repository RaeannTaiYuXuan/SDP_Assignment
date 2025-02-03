using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDP_Assignment.RAEANN
{
    public class CommandManager
    {
        private Stack<ICommand> commandHistory = new Stack<ICommand>();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            commandHistory.Push(command);
        }

        public void Undo()
        {
            if (commandHistory.Any())
            {
                ICommand command = commandHistory.Pop();
                command.Undo();
            }
            else
            {
                Console.WriteLine("No actions to undo.");
            }
        }
    }
}
