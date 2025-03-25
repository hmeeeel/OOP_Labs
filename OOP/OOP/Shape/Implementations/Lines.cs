using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
//using Line = System.Windows.Shapes.Line;
using System.Windows.Shapes;
using OOP.AbstractClasses;
namespace OOP.Shape.Implementations
{
    public class Lines : ShapeBase
    {
        public Point PositionEnd { get; set; }
        public Lines(Brush color, int width, Point start, Point end)
        {
            PenColor = color;
            PenWidth = width;
            PositionStart = start;
            PositionEnd = end;
        }

        public override void Draw(Canvas canvas)
        {
            Line line = new Line
            {
                X1 = PositionStart.X,
                Y1 = PositionStart.Y,
                X2 = PositionEnd.X,
                Y2 = PositionEnd.Y,
                Stroke = PenColor,
                StrokeThickness = PenWidth
            };

            canvas.Children.Add(line);
        }
    }
}
