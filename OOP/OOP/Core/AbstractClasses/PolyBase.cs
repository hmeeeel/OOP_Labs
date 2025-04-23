using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
namespace OOP.Core.AbstractClasses
{
    public abstract class PolyBase : ShapeBase
    {
        public List<Point> Points { get; set; }

        protected PolyBase(Brush color, int penWidth, List<Point> points)
        {
            PenColor = color;
            PenWidth = penWidth;

            if (points.Count > 0)
                PositionStart = points[0]; // 1ая точка
            else
                PositionStart = new Point();

            Points = points;
            Fill = null;
        }

        protected PolyBase(Brush color, int penWidth, List<Point> points, Brush fill)
    : this(color, penWidth, points)  
        {
            Fill = fill;  
        }

        protected PointCollection CreatePointCollection()
        {
            PointCollection pointCollection = new PointCollection();
            foreach (var point in Points)
            {
                pointCollection.Add(point);
            }
            return pointCollection;
        }
        public abstract override void Draw(Canvas canvas);
        protected abstract void UpdatePoints();
        public override void StartDraw(Point startPoint)
        {
            base.StartDraw(startPoint);
            Points = new List<Point> { startPoint };
            UpdatePoints();
        }

        // перетаскиваю посл точку
        public override void UpdateDraw(Point newPoint)
        {
            if (!IsDrawing) return;

            if (Points.Count > 0)
            {
                // не добавляем новую точку, а просто обновляем последнюю
                if (Points.Count > 1)
                    Points[Points.Count - 1] = newPoint;
                else
                    Points.Add(newPoint);

                UpdatePoints();
            }
        }
        public void ContinueDrawing(Point newPoint)
        {
            Points.Add(newPoint);
            UpdatePoints();
        }
        protected abstract void FinishShape();
        public override bool HandleMouseDown(Point point, int clickCount)
        {
            if (!IsDrawing)
            {
                StartDraw(point);
                return true;
            }

            if (clickCount > 1)
            {
                FinishShape();
                EndDraw();
                return false; 
            }

            ContinueDrawing(point);
            return true; 
        }
        public override bool IsOneClick()
        {
            return true; 
        }
        protected virtual void CompleteDrawing()
        {
        }
    }
}
