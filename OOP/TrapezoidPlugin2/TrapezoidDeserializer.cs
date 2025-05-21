using OOP.Core.AbstractClasses;
using OOP.Core.Interfaces;
using OOP.Services.SerAndDeser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TrapezoidPlugin
{
    public class TrapezoidDeserializer : DeserializerBase
    {
        public override string Name => "Trapezoid";

        public override IDraw Deserialize(SerializableShape shape)
        {
            var (penColor, penWidth, fillColor) = GetCommonProperties(shape);

            Point startPoint = ShapeDeserializer.DoubleArrayToPoint(shape.StartPoint);

            double topWidth = shape.Width * 0.7;

            if (shape.CustomProperties != null && shape.CustomProperties.ContainsKey("TopWidth"))
            {
                var topWidthObj = shape.CustomProperties["TopWidth"];
                topWidth = topWidthObj as double? ?? Convert.ToDouble(topWidthObj);
            }

            return new Trapezoid(
                penColor,
                penWidth,
                startPoint,
                shape.Width,
                shape.Height,
                topWidth,
                fillColor
            );
        }
    }
}
