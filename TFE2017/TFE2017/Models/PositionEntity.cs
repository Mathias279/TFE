using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models
{
    public class PositionEntity
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PositionEntity(double x, double y)
        {
            X = x;
            Y = y;
        }

        public new string ToString()
        {
            return string.Join(" ", this.X.ToString(), this.Y.ToString());
        }
    }
}
