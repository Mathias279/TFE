using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models
{
    public class PositionEntity
    {
        public float PosX { get; set; }
        public float PosY { get; set; }


        public string ToString()
        {
            return string.Join(" ", this.PosX.ToString(), this.PosX.ToString());
        }
    }
}
