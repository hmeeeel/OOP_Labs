using Newtonsoft.Json;
using OOP.Core.AbstractClasses;
using OOP.Core.Interfaces;
using OOP.Shape.Factory;
using OOP.Shape.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OOP.Services.SerAndDeser
{
    public class ShapeDeserializer
    {
        private static Dictionary<string, IDeserializer> deserializers = new Dictionary<string, IDeserializer>();

        static ShapeDeserializer()
        {
            RegisterDeserializers();
        }

        private static void RegisterDeserializers()
        {
            var deserializerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IDeserializer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in deserializerTypes)
            {
                var deserializer = (IDeserializer)Activator.CreateInstance(type);
                deserializers[deserializer.Name] = deserializer;

            }
        }

        public static void RegisterPluginDeserializer(IDeserializer deserializer)
        {
            if (deserializer != null) deserializers[deserializer.Name] = deserializer;
            
        }

        public static List<IDraw> LoadFromFile(string filePath, Canvas canvas)
        {
            List<IDraw> loadedShapes = new List<IDraw>();

            if (!File.Exists(filePath)) return loadedShapes;

            try
            {
                string jsonString = File.ReadAllText(filePath);
                var serializableShapes = JsonConvert.DeserializeObject<List<SerializableShape>>(jsonString);

                foreach (var serializableShape in serializableShapes)
                {
                    IDraw shape = DeserializeShape(serializableShape);
                    if (shape != null)
                    {
                        shape.Draw(canvas);
                        loadedShapes.Add(shape);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки файла: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return loadedShapes;
        }

        private static bool warning= false;
        private static IDraw DeserializeShape(SerializableShape serializableShape)
        {
            if (serializableShape == null || string.IsNullOrEmpty(serializableShape.Type)) return null;

            if (deserializers.TryGetValue(serializableShape.Type, out var deserializer))
            {
                return deserializer.Deserialize(serializableShape);
            }

            if (!warning)
            {
                MessageBox.Show($"Не найден десериализатор для типа: {serializableShape.Type}", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                warning = true;
            }
            return null;
        }

        public static Brush HexToBrush(string hex)
        {
            if (string.IsNullOrEmpty(hex)) return Brushes.Transparent;

            if (ColorConverter.ConvertFromString(hex) is Color color) return new SolidColorBrush(color);

            return Brushes.Black;
        }

        public static Point DoubleArrayToPoint(double[] array)
        {
            if (array == null || array.Length < 2) return new Point(0, 0);

            return new Point(array[0], array[1]);
        }

        public static List<Point> ConvertToPointList(List<double[]> points)
        {
            if (points == null) return new List<Point>();

            return points.Select(p => DoubleArrayToPoint(p)).ToList();
        }
    }
}