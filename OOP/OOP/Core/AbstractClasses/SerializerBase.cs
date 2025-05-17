using OOP.Core.Interfaces;
using OOP.Services.SerAndDeser;
using OOP.Shape.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OOP.Core.AbstractClasses
{
    public abstract class SerializerBase : ISerializer
    {
        public abstract string Name { get; }
        protected SerializableShape CreateBaseShape(ShapeBase shape)
        {
            return new SerializableShape
            {
                Type = shape.GetType().Name,
                PenColor = ShapeSerializer.ColorToHex(shape.PenColor ?? Brushes.Black),
                PenWidth = shape.PenWidth,
                Fill = ShapeSerializer.ColorToHex(shape.Fill ?? Brushes.Transparent)
            };
        }
        public abstract SerializableShape Serialize(IDraw shape);
    }

    public class LinesSerializer : SerializerBase
    {
        public override string Name => "Lines";
        public override SerializableShape Serialize(IDraw shape)
        {
            var line = shape as Lines;

            var serializableShape = CreateBaseShape(line);
            serializableShape.StartPoint = new double[] { line.PositionStart.X, line.PositionStart.Y };
            serializableShape.EndPoint = new double[] { line.PositionEnd.X, line.PositionEnd.Y };

            return serializableShape;
        }
    }

    public class RectanglesSerializer : SerializerBase
    {
        public override string Name => "Rectangles";
        public override SerializableShape Serialize(IDraw shape)
        {
            var rect = shape as Rectangles;

            var serializableShape = CreateBaseShape(rect);
            serializableShape.StartPoint = new double[] { rect.PositionStart.X, rect.PositionStart.Y };
            serializableShape.Width = rect.Width;
            serializableShape.Height = rect.Height;

            return serializableShape;
        }
    }

    public class EllipsesSerializer : SerializerBase
    {
        public override string Name => "Ellipses";
        public override SerializableShape Serialize(IDraw shape)
        {
            var ellipse = shape as Ellipses;

            var serializableShape = CreateBaseShape(ellipse);
            serializableShape.StartPoint = new double[] { ellipse.PositionStart.X, ellipse.PositionStart.Y };
            serializableShape.Width = ellipse.Width;
            serializableShape.Height = ellipse.Height;

            return serializableShape;
        }
    }

    public class PolylinesSerializer : SerializerBase
    {
        public override string Name => "Polylines";
        public override SerializableShape Serialize(IDraw shape)
        {
            var polyline = shape as Polylines;

            var serializableShape = CreateBaseShape(polyline);
            serializableShape.Points = polyline.Points.Select(p => new double[] { p.X, p.Y }).ToList();

            return serializableShape;
        }
    }

    public class PolygonsSerializer : SerializerBase
    {
        public override string Name => "Polygons";
        public override SerializableShape Serialize(IDraw shape)
        {
            var polygon = shape as Polygons;

            var serializableShape = CreateBaseShape(polygon);
            serializableShape.Points = polygon.Points.Select(p => new double[] { p.X, p.Y }).ToList();

            return serializableShape;
        }
    }
}
