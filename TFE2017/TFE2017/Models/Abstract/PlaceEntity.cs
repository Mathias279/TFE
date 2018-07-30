using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models.Abstract
{
    class PlaceEntity:MapEntity
    {
        //public double Distance { get; set; }
        public double Accessibility { get; set; }
        public PositionEntity Coordonates { get; set; }
    }
}