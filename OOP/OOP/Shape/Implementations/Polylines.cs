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
using OOP.Shape.Base;

namespace OOP.Shape.Implementations
{
    public class Polylines : PointCollections
    {
        public Polylines(Brush color, int penWidth, List<Point> points)
            : base(color, penWidth, points)
        {
        }

        public override void Draw(Canvas canvas)
        {
            Polyline polyline = new Polyline
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth
            };

            polyline.Points = CreatePointCollection();
            canvas.Children.Add(polyline); 
        }
    }
}
