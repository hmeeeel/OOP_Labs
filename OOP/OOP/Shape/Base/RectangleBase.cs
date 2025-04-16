using OOP.Core.AbstractClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
namespace OOP.Shape.Base
{
   public abstract class RectangleBase : ShapeBase
    {
        public double Width { get; set; }
        public double Height { get; set; }
        protected System.Windows.Shapes.Shape shapeElement;
        protected Point originalStart;

        protected RectangleBase(Brush color, int penWidth, Point start, double width, double height, Brush fill)
        {
            PenColor = color;
            PenWidth = penWidth;
            PositionStart = start;
            Width = width;
            Height = height;
            Fill = fill;

            InitializeShape();
        }

        protected abstract void InitializeShape();

        public override void Draw(Canvas canvas)
        {
            if (!canvas.Children.Contains(shapeElement))
            {
                canvas.Children.Add(shapeElement);
            }
        }

        public override void StartDraw(Point startPoint)
        {
            base.StartDraw(startPoint);
            originalStart = startPoint;
            Width = 0;
            Height = 0;
            UpdateShapePosition();
        }

        public override void UpdateDraw(Point newPoint)
        {
            if (!IsDrawing) return;

            double x = Math.Min(originalStart.X, newPoint.X);
            double y = Math.Min(originalStart.Y, newPoint.Y);
            Width = Math.Abs(newPoint.X - originalStart.X);
            Height = Math.Abs(newPoint.Y - originalStart.Y);

            PositionStart = new Point(x, y);
            UpdateShapePosition();
        }

        protected void UpdateShapePosition()
        {
            if (shapeElement != null)
            {
                shapeElement.Width = Width;
                shapeElement.Height = Height;
                Canvas.SetLeft(shapeElement, PositionStart.X);
                Canvas.SetTop(shapeElement, PositionStart.Y);
            }
        }
    }
}
