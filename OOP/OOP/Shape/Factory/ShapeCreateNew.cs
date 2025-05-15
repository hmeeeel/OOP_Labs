using OOP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using OOP.Core.AbstractClasses;

namespace OOP.Shape.Factory
{
    public class ShapeCreateNew
    {
        private static Dictionary<string, ConstructorInfo> shapeInfo;
        private static Dictionary<string, IPlugin> pluginShapes;
        static ShapeCreateNew()
        {
            shapeInfo = GetShapeInfo(); 
            pluginShapes = new Dictionary<string, IPlugin>();
        }

        public static Dictionary<string, ConstructorInfo> GetShapeInfo()
        {
            var constructors = new Dictionary<string, ConstructorInfo>();
            Assembly assembly = Assembly.GetExecutingAssembly(); //текущая сборка

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.Namespace == "OOP.Shape.Implementations")
                {
                    string shapeName = type.Name;

                    ConstructorInfo constructor = type.GetConstructors().FirstOrDefault(); //получается первый конструктор класса
                    if (constructor != null)
                    {
                        constructors[shapeName] = constructor;
                    }
                }
            }
            return constructors;
        }

        public static IDraw CreateShape(string shapeName, Brush penColor, int penWidth, Point startPoint, Brush fillColor)
        {

            if (shapeInfo.TryGetValue(shapeName, out ConstructorInfo constructor))
            {
                // фигня!!
                switch (shapeName)
                {
                    case "Lines":
                        return (IDraw)constructor.Invoke(new object[] { penColor, penWidth, startPoint, startPoint });
                    case "Rectangles":
                    case "Ellipses":
                        return (IDraw)constructor.Invoke(new object[] { penColor, penWidth, startPoint, 0.0, 0.0, fillColor });
                    case "Polylines":
                        return (IDraw)constructor.Invoke(new object[] { penColor, penWidth, new List<Point> { startPoint } });
                    case "Polygons":
                        return (IDraw)constructor.Invoke(new object[] { penColor, penWidth, new List<Point> { startPoint }, fillColor });
                    default:
                        throw new ArgumentException($"'{shapeName}' нет.");
                }
            }
            if (pluginShapes.TryGetValue(shapeName, out IPlugin plugin))
            {
                IDraw shape = plugin.CreateShape();
                if (shape is ShapeBase shapeBase)
                {
                    shapeBase.PenColor = penColor;
                    shapeBase.PenWidth = penWidth;
                    shapeBase.PositionStart = startPoint;
                    shapeBase.Fill = fillColor;
                }

                return shape;
            }

            throw new ArgumentException($"'{shapeName}' нет.");
        }

        public static List<string> GetAvailableShapes()
        {
            //return shapeInfo.Keys.ToList();
            var shapes = new List<string>(shapeInfo.Keys);
            shapes.AddRange(pluginShapes.Keys);
            return shapes;
        }

        public static void RegisterPlugin(IPlugin plugin)
        {
            if (!pluginShapes.ContainsKey(plugin.Name))
            {
                pluginShapes.Add(plugin.Name, plugin);
            }
        }
    }
}
