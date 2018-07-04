using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models
{
    class BuildingEntity
    {
        public string Id { get; private set; }
        public List<FloorEntity> FloorsList;
    }
}
