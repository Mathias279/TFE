using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models
{
    class RoomEntity
    {
        public List<PositionEntity> CornersList { get; set; }
        public List<PositionEntity> CallsList { get; set; }
        public List<DoorEntity> DoorsList { get; set; }

        public RoomEntity()
        {
        }

        public RoomEntity(List<PositionEntity> cornersList, List<PositionEntity> callsList, List<DoorEntity> doorsList)
        {
            CornersList = cornersList;
            CallsList = callsList;
            DoorsList = doorsList;
        }
    }
}
