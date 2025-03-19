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
    public class Polygons : PointCollections
    {
        public Polygons(Brush color, int penWidth, List<Point> points, Brush fill)
            : base(color, penWidth, points, fill)
        {
        }

        public Polygons(Brush color, int penWidth, List<Point> points)
            : base(color, penWidth, points) 
        {
        }

        public override void Draw(Canvas canvas)
        {
            Polygon polygon = new Polygon
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth,
                Fill = Fill,
            };

            polygon.Points = CreatePointCollection();
            canvas.Children.Add(polygon);
        }
    }
}
