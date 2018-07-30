using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Abstract;

namespace TFE2017.Core.Models
{
    class RoomEntity : PlaceEntity
    {
        public RoomEntity(string id, string name, PositionEntity position, double accessibilite)
        {
            Id = id;
            Name = name;
            Coordonates = position;
            Accessibility = accessibilite;
        }
    }
}
