using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
namespace OOP
{
    public class Polygons: ShapeBase
    {
        public List<Point> Points { get; set; }

        public Polygons(Brush color, int penWidth, List<Point> points)
        {

            this.PenColor = color;
            this.PenWidth = penWidth;

            if (points.Count > 0)
                this.PositionStart = points[0];
            else
                this.PositionStart = new Point();

            this.Points = points;
        }

        public override void Draw(Canvas canvas)
        {
            Polygon polygon = new Polygon
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth
            };

            PointCollection pointCollection = new PointCollection();
            foreach (var point in Points)
            {
                pointCollection.Add(point);
            }
            polygon.Points = pointCollection;

            canvas.Children.Add(polygon);
        }
    }
}
