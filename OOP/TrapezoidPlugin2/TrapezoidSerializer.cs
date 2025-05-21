using OOP.Core.AbstractClasses;
using OOP.Core.Interfaces;
using OOP.Services.SerAndDeser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrapezoidPlugin
{
    public class TrapezoidSerializer : SerializerBase
    {
        public override string Name => "Trapezoid";

        public override SerializableShape Serialize(IDraw shape)
        {
            var trapezoid = shape as Trapezoid;

            var serializableShape = CreateBaseShape((ShapeBase)shape);
            serializableShape.StartPoint = new double[] { trapezoid.PositionStart.X, trapezoid.PositionStart.Y };
            serializableShape.Width = trapezoid.Width;
            serializableShape.Height = trapezoid.Height;

            if (serializableShape.CustomProperties == null)
                serializableShape.CustomProperties = new Dictionary<string, object>();

            serializableShape.CustomProperties["TopWidth"] = trapezoid.TopWidth;

            return serializableShape;
        }
    }
}
