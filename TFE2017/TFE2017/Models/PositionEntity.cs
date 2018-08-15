using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models
{
    public class PositionEntity
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public PositionEntity(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
