using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using OOP.Core.Interfaces;
using System.Reflection.Metadata.Ecma335;


namespace OOP.Core.AbstractClasses
{
    public abstract class ShapeBase : IDraw
    {
        public Brush PenColor { get; set; }
        public int PenWidth { get; set; }
        public Point PositionStart { get; set; }
        public abstract void Draw(Canvas canvas);
        public Brush Fill { get; set; }
        protected bool IsDrawing { get; set; }
        public virtual void StartDraw(Point startPoint)
        {
            PositionStart = startPoint; // нач = кон
            IsDrawing = true;
        }
        public virtual void UpdateDraw(Point newPoint)
        {
        }
        public virtual void EndDraw()
        {
            IsDrawing = false;
        }
        public virtual bool HandleMouseDown(Point point, int clickN)
        {
            if (IsDrawing)
            {
                if (clickN > 1 && IsOneClick())
                {
                    EndDraw();
                    return false;
                }
                return true;
            }
            else
            {
                StartDraw(point);
                return true;
            }
        }
        public virtual bool IsOneClick()
        {
            return false;
        }
    }

}
