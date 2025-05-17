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
        private static readonly Dictionary<string, Type> shapeInfo;
        private static readonly Dictionary<string, IPlugin> pluginShapes;

        static ShapeCreateNew()
        {
            shapeInfo = GetShapeInfo();
            pluginShapes = new Dictionary<string, IPlugin>();
        }

        private static Dictionary<string, Type> GetShapeInfo()
        {
            var shapeTypes = new Dictionary<string, Type>();
            Assembly assembly = Assembly.GetExecutingAssembly();
            var shapeBaseType = typeof(ShapeBase);

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract && type.Namespace == "OOP.Shape.Implementations" && shapeBaseType.IsAssignableFrom(type)) 
                { 
                    shapeTypes[type.Name] = type;
                }
            }

            return shapeTypes;
        }

        public static IDraw CreateShape(string shapeName, Brush penColor, int penWidth, Point startPoint, Brush fillColor)
        {
            if (shapeInfo.TryGetValue(shapeName, out Type shapeType))
            {
                return CreateShapeInstance(shapeType, penColor, penWidth, startPoint, fillColor);
            }

            if (pluginShapes.TryGetValue(shapeName, out IPlugin plugin))
            {
                return plugin.CreateShape(penColor, penWidth, startPoint, fillColor);
            }

            throw new ArgumentException($"Фигура '{shapeName}' не найдена.");
        }

        private static IDraw CreateShapeInstance(Type shapeType, Brush penColor, int penWidth, Point startPoint, Brush fillColor)
        {
            var constructor = FindConstructor(shapeType);
            var parameters = ConstructorParam(constructor, penColor, penWidth, startPoint, fillColor);

            return (IDraw)constructor.Invoke(parameters);
        }

        private static ConstructorInfo FindConstructor(Type shapeType)
        {
            var constructors = shapeType.GetConstructors();
            return constructors.FirstOrDefault();
        }

        private static object[] ConstructorParam(ConstructorInfo constructor, Brush penColor, int penWidth, Point startPoint, Brush fillColor)
        {
            var parameters = constructor.GetParameters();
            var args = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = GetParameterValue(parameters[i], penColor, penWidth, startPoint, fillColor);
            }

            return args;
        }

        private static object GetParameterValue(ParameterInfo param, Brush penColor, int penWidth, Point startPoint, Brush fillColor)
        {
            Type paramType = param.ParameterType;
            var paramName = param.Name?.ToLower() ?? "";

            if (paramType == typeof(Brush) && (paramName.Contains("fill"))) return fillColor;
            if (paramType == typeof(Brush) && (paramName.Contains("stroke") || paramName.Contains("color")))return penColor;
            if (paramType == typeof(int)) return penWidth;
            if (paramType == typeof(Point)) return startPoint;
            if (paramType == typeof(double)) return 0.0;
            if (paramType == typeof(List<Point>)) return new List<Point> { startPoint };
           
            if (paramType.IsValueType) return Activator.CreateInstance(paramType);
            return null;
        }

        public static List<string> GetAvailableShapes()
        {
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