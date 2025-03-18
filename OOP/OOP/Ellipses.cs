using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
namespace OOP
{
     public class Ellipses : RectangleBase
    {
        public Ellipses(Brush color, int penWidth, Point start, double width, double height)
                        : base(color, penWidth, start, width, height)
        {}

        public override void Draw(Canvas canvas)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = Width,
                Height = Height,
                Stroke = PenColor,
                StrokeThickness = PenWidth
            };

            Canvas.SetLeft(ellipse, PositionStart.X);
            Canvas.SetTop(ellipse, PositionStart.Y);

            canvas.Children.Add(ellipse);
        }
    }
}
