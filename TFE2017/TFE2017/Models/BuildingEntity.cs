using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Abstract;

namespace TFE2017.Core.Models
{
    class BuildingEntity: MapEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        //public PositionEntity Coordonates { get; private set; }

        public BuildingEntity()
        {
        }
    }
}