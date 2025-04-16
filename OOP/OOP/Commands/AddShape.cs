using OOP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OOP.Commands
{
    public class AddShape : ICommand
    {
        private Canvas canvas;
        private IDraw shape;
        private List<IDraw> shapes;

        public AddShape(Canvas canvas, IDraw shape, List<IDraw> shapes)
        {
            this.canvas = canvas;
            this.shape = shape;
            this.shapes = shapes;
        }

        //Добавляет фигуру в список и рисует её при выполнении
        public void Execute()
        {
            shapes.Add(shape);
            shape.Draw(canvas); 
        }

        // Удаляет фигуру из списка и перерисовывает холст при отмене
        public void Undo()
        {
            shapes.Remove(shape);
            canvas.Children.Clear();
            foreach (var s in shapes)
            {
                s.Draw(canvas);
            }
        }
    }
} 