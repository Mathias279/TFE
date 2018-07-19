using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models.Interfaces
{
    interface IMapEntity
    {
        string Id { get; set; }
        string Name { get; set; }
        double PositionX { get; set; }
        double PositionY { get; set; }
    }
}