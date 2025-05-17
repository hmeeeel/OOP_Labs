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
    public abstract class DeserializerBase : IDeserializer
    {
        public abstract string Name { get; }
        public abstract IDraw Deserialize(SerializableShape serializableShape);

        protected (Brush penColor, int penWidth, Brush fillColor) GetCommonProperties(SerializableShape shape)
        {
            Brush penColor = ShapeDeserializer.HexToBrush(shape.PenColor);
            int penWidth = shape.PenWidth;
            Brush fillColor = ShapeDeserializer.HexToBrush(shape.Fill);

            return (penColor, penWidth, fillColor);
        }
    }

    public class LinesDeserializer : DeserializerBase
    {
        public override string Name => "Lines";
        public override IDraw Deserialize(SerializableShape shape)
        {
            var (penColor, penWidth, _) = GetCommonProperties(shape);

            return new Lines(
                penColor,
                penWidth,
                ShapeDeserializer.DoubleArrayToPoint(shape.StartPoint),
                ShapeDeserializer.DoubleArrayToPoint(shape.EndPoint)
            );
        }
    }

    public class RectanglesDeserializer : DeserializerBase
    {
        public override string Name => "Rectangles";
        public override IDraw Deserialize(SerializableShape shape)
        {
            var (penColor, penWidth, fillColor) = GetCommonProperties(shape);

            return new Rectangles(
                penColor,
                penWidth,
                ShapeDeserializer.DoubleArrayToPoint(shape.StartPoint),
                shape.Width,
                shape.Height,
                fillColor
            );
        }
    }

    public class EllipsesDeserializer : DeserializerBase
    {
        public override string Name => "Ellipses";
        public override IDraw Deserialize(SerializableShape shape)
        {
            var (penColor, penWidth, fillColor) = GetCommonProperties(shape);

            return new Ellipses(
                penColor,
                penWidth,
                ShapeDeserializer.DoubleArrayToPoint(shape.StartPoint),
                shape.Width,
                shape.Height,
                fillColor
            );
        }
    }

    public class PolylinesDeserializer : DeserializerBase
    {
        public override string Name => "Polylines";
        public override IDraw Deserialize(SerializableShape shape)
        {
            var (penColor, penWidth, _) = GetCommonProperties(shape);

            return new Polylines(
                penColor,
                penWidth,
                ShapeDeserializer.ConvertToPointList(shape.Points)
            );
        }
    }
    public class PolygonsDeserializer : DeserializerBase
    {
        public override string Name => "Polygons";
        public override IDraw Deserialize(SerializableShape shape)
        {
            var (penColor, penWidth, fillColor) = GetCommonProperties(shape);

            return new Polygons(
                penColor,
                penWidth,
                ShapeDeserializer.ConvertToPointList(shape.Points),
                fillColor
            );
        }
    }
}
