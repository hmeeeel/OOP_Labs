using OOP.AbstractClasses;
using System.Windows;
using System.Windows.Media;

namespace OOP.Shape.Base
{
   public abstract class RectangleBase : ShapeBase
    {
        public double Width { get; set; }
        public double Height { get; set; }

        protected RectangleBase(Brush color, int penWidth, Point start, double width, double height, Brush fill)
        {
            PenColor = color;
            PenWidth = penWidth;
            PositionStart = start;
            Width = width;
            Height = height;
            Fill = fill;
        }
    }
}
