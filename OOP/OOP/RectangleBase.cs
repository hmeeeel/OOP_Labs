
using System.Windows;
using System.Windows.Media;

namespace OOP
{
   public abstract class RectangleBase : ShapeBase
    {
        public double Width { get; set; }
        public double Height { get; set; }

        protected RectangleBase(Brush color, int penWidth, Point start, double width, double height, Brush fill)
        {
            this.PenColor = color;
            this.PenWidth = penWidth;
            this.PositionStart = start;
            this.Width = width;
            this.Height = height;
            this.Fill = fill;
        }
    }
}
