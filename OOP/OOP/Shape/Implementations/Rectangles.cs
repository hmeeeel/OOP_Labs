using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
using OOP.Core.AbstractClasses;

namespace OOP.Shape.Implementations
{
    public class Rectangles : RectangleBase
    {
        public Rectangles(Brush color, int penWidth, Point start, double width, double height, Brush fill)
           : base(color, penWidth, start, width, height, fill)
        {
        }

        protected override void InitializeShape()
        {
            var rectangle = new Rectangle
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth,
                Fill = Fill,
                Width = Width,
                Height = Height
            };

            Canvas.SetLeft(rectangle, PositionStart.X);
            Canvas.SetTop(rectangle, PositionStart.Y);

            shapeElement = rectangle;
        }
    }
}
