using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using static OOP.Core.Constants.Constants;
using OOP.Core.AbstractClasses;

namespace OOP.Shape.Implementations
{
    public class Polylines : PolyBase
    {
        private Polyline polyline;

        public Polylines(Brush color, int penWidth, List<Point> points)
            : base(color, penWidth, points)
        {
            polyline = new Polyline
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth
            };
            UpdatePoints();
        }

        // как карандаш в PAINT - доб каждую новую точку во время движения мыши
        //public override void UpdateDraw(Point newPoint)
        //{
        //    if (!IsDrawing) return;
        //    if (Points.Count > 0 && (Points[Points.Count - 1].X != newPoint.X || Points[Points.Count - 1].Y != newPoint.Y))
        //    {
        //        Points.Add(newPoint);
        //        UpdatePoints();
        //    }
        //}

        protected override void UpdatePoints()
        {
            polyline.Points = CreatePointCollection();
        }
        public override void Draw(Canvas canvas)
        {
            if (!canvas.Children.Contains(polyline)) canvas.Children.Add(polyline);
        }
        protected override void FinishShape()
        {
        }
    }
}
