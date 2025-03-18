using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace OOP
{
    public class Polylines : ShapeBase
    {
        public List<Point> Points { get; set; }
        public Polylines(Brush color, int penWidth, List<Point> points)
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
            Polyline polyline = new Polyline
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth
            };

            PointCollection pointCollection = new PointCollection();
            foreach (var point in Points)
            {
                pointCollection.Add(point);  
            }

            polyline.Points = pointCollection;  
            canvas.Children.Add(polyline); 
        }
    }
}
