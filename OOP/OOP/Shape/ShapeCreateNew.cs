using OOP.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace OOP.Shape
{
    public class ShapeCreateNew
    {
        private static Dictionary<string, ConstructorInfo> shapeInfo;

        static ShapeCreateNew()
        {
            shapeInfo = GetShapeInfo(); // 1 
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
                    case "Line":
                        return (IDraw)constructor.Invoke(new object[] { penColor, penWidth, startPoint, startPoint });
                    case "Rectangles":
                    case "Ellipses":
                        return (IDraw)constructor.Invoke(new object[] { penColor, penWidth, startPoint, 0.0, 0.0, fillColor });
                    case "Polylines":
                        return (IDraw)constructor.Invoke(new object[] { penColor, penWidth, new List<Point> { startPoint } });
                    case "Polygons":
                        return (IDraw)constructor.Invoke(new object[] { penColor, penWidth, new List<Point> { startPoint }, fillColor });
                    default:
                        throw new ArgumentException($"Shape '{shapeName}' not supported.");
                }
            }

            throw new ArgumentException($"'{shapeName}' нет.");
        }

        public static List<string> GetAvailableShapes()
        {
            return shapeInfo.Keys.ToList();
        }
    }
}
