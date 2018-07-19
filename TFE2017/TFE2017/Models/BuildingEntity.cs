using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Interfaces;

namespace TFE2017.Core.Models
{
    class BuildingEntity
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public double PositionX { get; private set; }
        public double PositionY { get; private set; }

        public BuildingEntity()
        {
        }
    }
}