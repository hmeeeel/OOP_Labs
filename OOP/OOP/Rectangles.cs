using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Shapes;
namespace OOP
{
    public class Rectangles : RectangleBase
    {
        //public Rectangles(Brush color, int penWidth, Point start, double width, double height)
        //{
        //    this.PenColor = color;
        //    this.PenWidth = penWidth;
        //    this.PositionStart = start;
        //    this.Width = width;
        //    this.Height = height;
        //}
        public Rectangles(Brush color, int penWidth, Point start, double width, double height, Brush fill)
                         : base(color, penWidth, start, width, height, fill)
        {}
        public override void Draw(Canvas canvas)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = Width,
                Height = Height,
                Stroke = PenColor,
                StrokeThickness = PenWidth,
                Fill = Fill
            };

            Canvas.SetLeft(rectangle, PositionStart.X);
            Canvas.SetTop(rectangle, PositionStart.Y);

            canvas.Children.Add(rectangle);
        }
    }
}
