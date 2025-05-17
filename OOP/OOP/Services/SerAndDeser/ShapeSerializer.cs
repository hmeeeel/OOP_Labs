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
using System.Reflection;

namespace OOP.Services.SerAndDeser
{
    public class ShapeSerializer
    {
        private static Dictionary<string, ISerializer> serializers = new Dictionary<string, ISerializer>();

        static ShapeSerializer()
        {
            RegisterSerializers();
        }

        private static void RegisterSerializers()
        {
            var serializerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(ISerializer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in serializerTypes)
            {
                var serializer = (ISerializer)Activator.CreateInstance(type);
                serializers[serializer.Name] = serializer;
            }
        }

        public static void SaveToFile(List<IDraw> shapes, string filePath)
        {
            var serializableShapes = shapes
                .Select(shape => ConvertToSer(shape))
                .Where(s => s != null)
                .ToList();

            string jsonString = JsonConvert.SerializeObject(serializableShapes, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }

        private static SerializableShape ConvertToSer(IDraw shape)
        {
            if (shape == null) return null;

            string typeName = shape.GetType().Name;

            ISerializer serializer = FindSerializer(shape);

            if (serializer != null) return serializer.Serialize(shape);
            else return CreateDefSer(shape);
        }

        private static ISerializer FindSerializer(IDraw shape)
        {
            string typeName = shape.GetType().Name;

            if (serializers.TryGetValue(typeName, out var serializer))
            {
                return serializer;
            }

            foreach (var entry in serializers)
            {
                Type baseType = Type.GetType(entry.Key) ?? AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(t => t.Name == entry.Key);

                if (baseType != null && baseType.IsAssignableFrom(shape.GetType()))
                {
                    return entry.Value;
                }
            }

            return null;
        }

        private static SerializableShape CreateDefSer(IDraw shape)
        {
            if (shape is ShapeBase shapeBase)
            {
                return new SerializableShape
                {
                    Type = shape.GetType().Name,
                    PenColor = ColorToHex(shapeBase.PenColor ?? Brushes.Black),
                    PenWidth = shapeBase.PenWidth,
                    Fill = ColorToHex(shapeBase.Fill ?? Brushes.Transparent)
                };
            }

            return new SerializableShape
            {
                Type = shape.GetType().Name
            };
        }

        public static string ColorToHex(Brush brush)
        {
            if (brush is SolidColorBrush solidBrush)
            {
                return $"#{solidBrush.Color.R:X2}{solidBrush.Color.G:X2}{solidBrush.Color.B:X2}";
            }
            return "#000000"; 
        }
        public static void RegisterPluginSerializer(ISerializer serializer)
        {
            if (serializer != null)
            {
                serializers[serializer.Name] = serializer;
            }
        }
    }
}