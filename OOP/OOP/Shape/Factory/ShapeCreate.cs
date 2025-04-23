using System.Windows.Media;
using System.Windows;
using OOP.Shape.Implementations;
using OOP.Core.Interfaces;

namespace OOP.Shape.Factory
{
    public class ShapeCreate
    {
        public IDraw CreateLine(Brush color, int penWidth, Point start, Point end)
        {
            return new Lines(color, penWidth, start, end);
        }

        public IDraw CreateRectangle(Brush color, int penWidth, Point start, double width, double height, Brush fill)
        {
            return new Rectangles(color, penWidth, start, width, height, fill);
        }

        public IDraw CreateEllipse(Brush color, int penWidth, Point start, double width, double height, Brush fill)
        {
            return new Ellipses(color, penWidth, start, width, height, fill) ;
        }
        public IDraw CreatePolyline(Brush color, int penWidth, List<Point> points)
        { 
            return new Polylines(color, penWidth, points);
        }
        public IDraw CreatePolygon(Brush color, int penWidth, List<Point> points, Brush fill)
        {
            return new Polygons(color, penWidth, points, fill);
        }
        // придумать че-то другое 
        public IDraw CreateShape(string shapeType, Brush color, int penWidth, Point start, Brush fill)
        {
            switch (shapeType)
            {
                case "Rectangle":
                    return CreateRectangle(color, penWidth, start, 0, 0, fill);
                case "Ellipse":
                    return CreateEllipse(color, penWidth, start, 0, 0, fill);
                case "Line":
                    return CreateLine(color, penWidth, start, start);
                case "Polyline":
                    return CreatePolyline(color, penWidth, new List<Point> { start });
                case "Polygon":
                    return CreatePolygon(color, penWidth, new List<Point> { start }, fill);
                default:
                    return null;
            }
        }
    }
}
