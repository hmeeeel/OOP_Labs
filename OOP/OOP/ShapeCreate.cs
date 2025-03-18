using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace OOP
{
    public class ShapeCreate
    {
        public IDraw CreateLine(Brush color, int penWidth, Point start, Point end)
        {
            return new Lines(color, penWidth, start, end);
        }

        public IDraw CreateRectangle(Brush color, int penWidth, Point start, double width, double height)
        {
            return new Rectangles(color, penWidth, start, width, height);
        }

        public IDraw CreateEllipse(Brush color, int penWidth, Point start, double width, double height)
        {
            return new Ellipses(color, penWidth, start, width, height);
        }
        public IDraw CreatePolyline(Brush color, int penWidth, List<Point> points)
        {
            return new Polylines(color, penWidth, points);
        }

        public IDraw CreatePolygon(Brush color, int penWidth, List<Point> points)
        {
            return new Polygons(color, penWidth, points);
        }
    }
}
