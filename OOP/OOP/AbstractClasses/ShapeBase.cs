﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using OOP.Interfaces;


namespace OOP.AbstractClasses
{
    public abstract class ShapeBase : IDraw
    {
        public Brush PenColor { get; set; }
        public int PenWidth { get; set; }
        public Point PositionStart { get; set; }
        public abstract void Draw(Canvas canvas);
  
        public Brush Fill { get; set; }
    }
}
