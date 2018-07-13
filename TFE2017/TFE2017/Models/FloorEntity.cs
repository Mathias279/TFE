using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models
{
    public class FloorEntity
    {
        public string Name { get; set; }
        public int FloorNbr { get; set; }
        private List<RoomEntity> RoomEntities { get; set; }
    }
}