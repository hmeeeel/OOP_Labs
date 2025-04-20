using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OOP.Core.Interfaces;

namespace OOP.Commands
{
    public class UndoOrRedoList
    {
        private List<ICommand> commandHistory = new List<ICommand>();

        private int currentIndex = -1; // первое нажатие с одной фигурой, потом исправить

        public void ExecuteCommand(ICommand command)
        {
            //удал все команды, которые были отменены
            if (currentIndex < commandHistory.Count - 1)
            {
                commandHistory.RemoveRange(currentIndex + 1, commandHistory.Count - currentIndex - 1);
            }

            command.Execute();

            commandHistory.Add(command);
            currentIndex++;
        }

        public bool Undo()
        {
            if (currentIndex >= 0)
            {
                commandHistory[currentIndex].Undo();
                currentIndex--;
                return true;
            }
            return false;
        }

        public bool Redo()
        {
            if (currentIndex < commandHistory.Count - 1)
            {
                currentIndex++;
                commandHistory[currentIndex].Execute();
                return true;
            }
            return false;
        }
    }
}
