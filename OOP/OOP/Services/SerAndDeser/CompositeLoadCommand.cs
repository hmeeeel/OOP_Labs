using OOP.Commands;
using OOP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OOP.Services.SerAndDeser
{
    public class CompositeCommand : ICommand
    {
        private readonly List<ICommand> commands = new List<ICommand>();
        public void AddCommand(ICommand command)
        {
            commands.Add(command);
        }
        public void Execute()
        {
            foreach (var command in commands)
            {
                command.Execute();
            }
        }
        public void Undo()
        {
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                commands[i].Undo();
            }
        }
    }
}