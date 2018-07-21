using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models.Abstract
{
    abstract class PlaceEntity:MapEntity
    {
        public abstract double Distance { get; set; }
        public abstract double Accessibility { get; set; }
        public abstract PositionEntity Coordonates { get; set; }
    }
}