using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using OOP.Shape.Base;
using static OOP.Core.Constants.Constants;
namespace OOP.Shape.Implementations
{
    public class Polygons : PointCollections
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

        private void UpdatePoints()
        {
            polygon.Points = CreatePointCollection();
        }

        public override void Draw(Canvas canvas)
        {
            if (!canvas.Children.Contains(polygon))
                canvas.Children.Add(polygon);
        }

        public override void StartDraw(Point startPoint)
        {
            base.StartDraw(startPoint);
            Points = new List<Point> { startPoint };
            UpdatePoints();
        }

        //перетаскиваю посл точку
        public override void UpdateDraw(Point newPoint)
        {
            if (!IsDrawing) return;

            if (Points.Count > 0)
            {
                // не добавляем новую точку, а просто обновляем посл
                if (Points.Count > 1)  Points[Points.Count - 1] = newPoint;
                else Points.Add(newPoint);

                UpdatePoints();
            }
        }

        public void ContinueDrawing(Point newPoint)
        {
            Points.Add(newPoint);
            UpdatePoints();
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
    }
}
