using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OOP.Core.Interfaces
{
    public interface IDraw
    {
        void Draw(Canvas canvas);
        void UpdateDraw(Point newPoint);
        void StartDraw(Point startPoint);
        void EndDraw();
        bool HandleMouseDown(Point point, int clickN);
        bool IsOneClick();
    }
}
