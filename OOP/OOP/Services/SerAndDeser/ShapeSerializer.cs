using OOP.Core.AbstractClasses;
using OOP.Core.Interfaces;
using OOP.Shape.Factory;
using OOP.Shape.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;

namespace OOP.Services.SerAndDeser
{
    public class ShapeSerializer
    {
        public static void SaveShapesToFile(List<IDraw> shapes, string filePath)
        {
            List<SerializableShape> serializableShapes = new List<SerializableShape>();

            foreach (IDraw shape in shapes)
            {
                SerializableShape serializableShape = ConvertToSerializableShape(shape);
                if (serializableShape != null) serializableShapes.Add(serializableShape);
            }

            string jsonString = JsonConvert.SerializeObject(serializableShapes, Formatting.Indented);

            File.WriteAllText(filePath, jsonString);
        }

        private static SerializableShape ConvertToSerializableShape(IDraw shape)
        {
            string shapeType = shape.GetType().Name;

            SerializableShape serializableShape = new SerializableShape
            {
                Type = shapeType,
                PenColor = ColorToHex(shape is ShapeBase ? ((ShapeBase)shape).PenColor : Brushes.Black),
                PenWidth = shape is ShapeBase ? ((ShapeBase)shape).PenWidth : 1,
                Fill = shape is ShapeBase ? ColorToHex(((ShapeBase)shape).Fill) : "#00000000"
            };

            if (shape is Lines)
            {
                Lines line = (Lines)shape;
                serializableShape.StartPoint = new double[] { line.PositionStart.X, line.PositionStart.Y };
                serializableShape.EndPoint = new double[] { line.PositionEnd.X, line.PositionEnd.Y };
            }
            else if (shape is RectangleBase)
            {
                RectangleBase rectBase = (RectangleBase)shape;
                serializableShape.StartPoint = new double[] { rectBase.PositionStart.X, rectBase.PositionStart.Y };
                serializableShape.Width = rectBase.Width;
                serializableShape.Height = rectBase.Height;
            }
            else if (shape is PolyBase)
            {
                PolyBase polyBase = (PolyBase)shape;
                serializableShape.Points = polyBase.Points.Select(p => new double[] { p.X, p.Y }).ToList();
            }

            return serializableShape;
        }

        private static string ColorToHex(Brush brush)
        {
            if (brush == null) return "#00000000"; 

            if (brush is SolidColorBrush solidBrush)
            {
                Color color = solidBrush.Color;
                return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
            }
            return "#00000000"; 
        }
    }  
}