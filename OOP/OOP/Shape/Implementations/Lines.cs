using System.Windows.Media;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Shapes;
using OOP.Core.AbstractClasses;
namespace OOP.Shape.Implementations
{
    public class Lines : ShapeBase
    {
        public Point PositionEnd { get; set; }
        private Line line;

        public Lines(Brush color, int width, Point start, Point end)
        {
            PenColor = color;
            PenWidth = width;
            PositionStart = start;
            PositionEnd = end;

            line = new Line
            {
                X1 = PositionStart.X,
                Y1 = PositionStart.Y,
                X2 = PositionEnd.X,
                Y2 = PositionEnd.Y,
                Stroke = PenColor,
                StrokeThickness = PenWidth
            };
        }

        public override void Draw(Canvas canvas)
        {
            if (!canvas.Children.Contains(line))
                canvas.Children.Add(line);
        }

        public override void StartDraw(Point startPoint)
        {
            base.StartDraw(startPoint);
            PositionStart = startPoint;
            PositionEnd = startPoint;

            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;
            line.X2 = startPoint.X;
            line.Y2 = startPoint.Y;
        }

        public override void UpdateDraw(Point newPoint)
        {
            if (!IsDrawing) return;

            PositionEnd = newPoint;
            line.X2 = newPoint.X;
            line.Y2 = newPoint.Y;
        }
    }
}
