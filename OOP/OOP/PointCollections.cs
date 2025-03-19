using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
namespace OOP
{
    public abstract class PointCollections : ShapeBase
    {
        public List<Point> Points { get; set; }

        protected PointCollections(Brush color, int penWidth, List<Point> points)
        {
            this.PenColor = color;
            this.PenWidth = penWidth;

            if (points.Count > 0)
                this.PositionStart = points[0];
            else
                this.PositionStart = new Point();

            this.Points = points;
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
    }
}
