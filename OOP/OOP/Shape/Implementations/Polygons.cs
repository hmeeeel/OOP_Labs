using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using static OOP.Core.Constants.Constants;
using OOP.Core.AbstractClasses;
namespace OOP.Shape.Implementations
{
    public class Polygons : PolyBase
    {
        private Polygon polygon;

        public Polygons(Brush color, int penWidth, List<Point> points, Brush fill)
            : base(color, penWidth, points, fill)
        {
            polygon = new Polygon
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth,
                Fill = Fill
            };
            UpdatePoints();
        }

        public Polygons(Brush color, int penWidth, List<Point> points)
            : this(color, penWidth, points, Brushes.Transparent)
        {}

        protected override void UpdatePoints()
        {
            polygon.Points = CreatePointCollection();
        }

        public override void Draw(Canvas canvas)
        {
            if (!canvas.Children.Contains(polygon))
                canvas.Children.Add(polygon);
        }

        //  замыкание 
        public void ClosePolygon()
        {
            if (Points.Count >= MIN_POLYGON_POINTS && (Points[0].X != Points[Points.Count - 1].X || Points[0].Y != Points[Points.Count - 1].Y))
            {
                Points.Add(Points[0]); // 1ая в конец
                UpdatePoints();
            }
        }

        protected override void FinishShape()
        {
            ClosePolygon();
        }
    }
}
