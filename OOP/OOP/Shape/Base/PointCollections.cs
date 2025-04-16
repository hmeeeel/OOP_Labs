using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using OOP.Core.AbstractClasses;
namespace OOP.Shape.Base
{
    public abstract class PointCollections : ShapeBase
    {
        public List<Point> Points { get; set; }

        protected PointCollections(Brush color, int penWidth, List<Point> points)
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

        protected PointCollections(Brush color, int penWidth, List<Point> points, Brush fill)
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
    }
}
