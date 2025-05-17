using OOP.Core.AbstractClasses; 
using OOP.Core.Interfaces;    
using OOP.Services;          
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes; 
using Newtonsoft.Json;
using OOP.Services.SerAndDeser;
using OOP.Shape.Implementations;
namespace TrapezoidPlugin
{
    public class TrapezoidPlugin : IPlugin
    {
        public string Name => "Trapezoid";
        public string Description => "ѕлагин дл€ создани€ трапеций";
        public Type ShapeType => typeof(Trapezoid);
        public IDraw CreateShape(Brush penColor, int penWidth, Point startPoint, Brush fillColor)
        {
            return new Trapezoid(penColor, penWidth, startPoint, 0, 0, 0, fillColor);
        }
    }

    public class Trapezoid : ShapeBase
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public double TopWidth { get; set; }
        private Polygon trapezoid;

        public Trapezoid(Brush color, int penWidth, Point start, double width, double height, double topWidth, Brush fill)
        {
            PenColor = color;
            PenWidth = penWidth;
            PositionStart = start;
            Width = width;
            Height = height;
            TopWidth = topWidth > 0 ? topWidth : width * 0.7;
            Fill = fill;
            InitializeShape();
        }

        private void InitializeShape()
        {
            trapezoid = new Polygon
            {
                Stroke = PenColor,
                StrokeThickness = PenWidth,
                Fill = Fill
            };
            UpdatePoints();
        }

        private void UpdatePoints()
        {
            PointCollection points = new PointCollection();
            double offset = (Width - TopWidth) / 2;
            points.Add(new Point(0, Height));           // н л
            points.Add(new Point(Width, Height));       // н п
            points.Add(new Point(Width - offset, 0));   // в п
            points.Add(new Point(offset, 0));           // в л
            trapezoid.Points = points;
        }

        public override void Draw(Canvas canvas)
        {
            if (!canvas.Children.Contains(trapezoid))
            {
                canvas.Children.Add(trapezoid);
            }
            Canvas.SetLeft(trapezoid, PositionStart.X);
            Canvas.SetTop(trapezoid, PositionStart.Y);
        }

        public override void StartDraw(Point startPoint)
        {
            base.StartDraw(startPoint);
            Width = 0;
            Height = 0;
            TopWidth = 0;
            UpdatePoints();
        }

        public override void UpdateDraw(Point newPoint)
        {
            if (!IsDrawing) return;
            Width = Math.Abs(newPoint.X - PositionStart.X);
            Height = Math.Abs(newPoint.Y - PositionStart.Y);
            TopWidth = Width * 0.7;

            if (newPoint.X < PositionStart.X || newPoint.Y < PositionStart.Y)
            {
                Point newStart = new Point(
                    Math.Min(PositionStart.X, newPoint.X),
                    Math.Min(PositionStart.Y, newPoint.Y)
                );
                PositionStart = newStart;
            }

            UpdatePoints();
        }
    }

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