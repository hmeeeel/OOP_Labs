using OOP.Services.SerAndDeser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OOP.Core.Interfaces
{
    public interface IPlugin
    {
        string Name { get; }
        string Description { get; }
        Type ShapeType { get; }
        IDraw CreateShape(Brush penColor, int penWidth, Point startPoint, Brush fillColor);
    }
}