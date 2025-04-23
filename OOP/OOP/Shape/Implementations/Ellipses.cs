using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using OOP.Core.AbstractClasses;
namespace OOP.Shape.Implementations
{
     public class Ellipses : RectangleBase
    {
        public Ellipses(Brush color, int penWidth, Point start, double width, double height, Brush fill)
            : base(color, penWidth, start, width, height, fill)
        {
        }

        protected override void InitializeShape()
        {
            var ellipse = new Ellipse
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth,
                Fill = Fill,
                Width = Width,
                Height = Height
            };

            Canvas.SetLeft(ellipse, PositionStart.X);
            Canvas.SetTop(ellipse, PositionStart.Y);

            shapeElement = ellipse;
        }
    }
}
