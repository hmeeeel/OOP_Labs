using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP.Services.SerAndDeser
{
    public class SerializableShape
    {
        public string Type { get; set; }
        public string PenColor { get; set; }
        public int PenWidth { get; set; }
        public string Fill { get; set; }
        public double[] StartPoint { get; set; }
        public double[] EndPoint { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public List<double[]> Points { get; set; }
    }
}
