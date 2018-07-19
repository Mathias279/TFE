using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models.Interfaces
{
    interface IPlaceEntity : IMapEntity
    {
        double Distance { get; set; }
        double Accessibility { get; set; }
    }
}