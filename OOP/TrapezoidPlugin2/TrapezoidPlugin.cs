// Файл: TrapezoidPlugin/Trapezoid.cs
using OOP.Core.AbstractClasses; // Путь к PolyBase
using OOP.Core.Interfaces;    // Путь к IDraw
using OOP.Services;           // Путь к JsonSerializationUtils
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes; // Для Polygon (внутренний элемент PolyBase)
using Newtonsoft.Json; // Для JsonIgnore, если потребуется
namespace TrapezoidPlugin
{
    public class TrapezoidPlugin : IPlugin
    {
        public string Name => "Trapezoid";
        public string Description => "A plugin that adds trapezoid shape functionality";
       // public string Version => "1.0.0";
        public Type ShapeType => typeof(Trapezoid);

        public IDraw CreateShape()
        {
            return new Trapezoid(Brushes.Black, 2, new Point(0, 0), 0, 0, 0, Brushes.Transparent);
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
            TopWidth = topWidth > 0 ? topWidth : width * 0.7; // Default top width if not provided
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

            // Bottom-left point
            points.Add(new Point(0, Height));
            // Bottom-right point
            points.Add(new Point(Width, Height));
            // Top-right point
            points.Add(new Point(Width - offset, 0));
            // Top-left point
            points.Add(new Point(offset, 0));

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
            TopWidth = Width * 0.7; // Make top width 70% of bottom width

            // Update position if drawing in negative direction
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
}