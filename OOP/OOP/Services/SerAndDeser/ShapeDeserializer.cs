using OOP.Core.AbstractClasses;
using OOP.Core.Interfaces;
using OOP.Shape.Factory;
using OOP.Shape.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OOP.Services.SerAndDeser
{
    public class ShapeDeserializer
    {

        public static List<IDraw> LoadFromFile(string filePath, Canvas canvas)
        {
            List<IDraw> loadedShapes = new List<IDraw>();

            if (!File.Exists(filePath)) return loadedShapes;

            try
            {
                string jsonString = File.ReadAllText(filePath);

                var serializableShapes = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SerializableShape>>(jsonString);

                foreach (var serializableShape in serializableShapes)
                {
                    IDraw shape = ConvertToShape(serializableShape);
                    if (shape != null)
                    {
                        shape.Draw(canvas);
                        loadedShapes.Add(shape);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ОШИБКА: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return loadedShapes;
        }

        private static IDraw ConvertToShape(SerializableShape serializableShape)
        {
            Brush penColor = HexToBrush(serializableShape.PenColor);
            Brush fillColor = HexToBrush(serializableShape.Fill);

            switch (serializableShape.Type)
            {
                case "Lines":
                    return new Lines(
                        penColor,
                        serializableShape.PenWidth,
                        new Point(serializableShape.StartPoint[0], serializableShape.StartPoint[1]),
                        new Point(serializableShape.EndPoint[0], serializableShape.EndPoint[1])
                    );

                case "Rectangles":
                    return new Rectangles(
                        penColor,
                        serializableShape.PenWidth,
                        new Point(serializableShape.StartPoint[0], serializableShape.StartPoint[1]),
                        serializableShape.Width,
                        serializableShape.Height,
                        fillColor
                    );

                case "Ellipses":
                    return new Ellipses(
                        penColor,
                        serializableShape.PenWidth,
                        new Point(serializableShape.StartPoint[0], serializableShape.StartPoint[1]),
                        serializableShape.Width,
                        serializableShape.Height,
                        fillColor
                    );

                case "Polylines":
                    return new Polylines(
                        penColor,
                        serializableShape.PenWidth,
                        ConvertToPointList(serializableShape.Points)
                    );

                case "Polygons":
                    return new Polygons(
                        penColor,
                        serializableShape.PenWidth,
                        ConvertToPointList(serializableShape.Points),
                        fillColor
                    );

                default:
                    MessageBox.Show($"--: {serializableShape.Type}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return null;
            }
        }

        private static List<Point> ConvertToPointList(List<double[]> points)
        {
            List<Point> result = new List<Point>();
            foreach (var point in points)
            {
                result.Add(new Point(point[0], point[1]));
            }
            return result;
        }

        private static Brush HexToBrush(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return Brushes.Transparent;

            try
            {
                return (Brush)new BrushConverter().ConvertFrom(hex);
            }
            catch
            {
                return Brushes.Black; 
            }
        }
    }
}