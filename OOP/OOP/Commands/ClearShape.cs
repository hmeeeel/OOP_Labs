using OOP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OOP.Commands
{
    public class ClearShape : ICommand
    {
        private readonly Canvas canvas;
        private readonly List<IDraw> shapes;
        private List<IDraw> savedShapes;

        public ClearShape(Canvas canvas, List<IDraw> shapes)
        {
            this.canvas = canvas;
            this.shapes = shapes;
            this.savedShapes = new List<IDraw>(shapes);
        }

        public void Execute()
        {
            savedShapes = new List<IDraw>(shapes);
            shapes.Clear();
            canvas.Children.Clear();
        }

        public void Undo()
        {
            shapes.Clear();
            shapes.AddRange(savedShapes);
            canvas.Children.Clear();
            foreach (var shape in shapes)
            {
                shape.Draw(canvas);
            }
        }
    }
}