using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models.Interfaces
{
    interface IRelationEntity : IMapEntity
    {
        double Width { get; set; }
    }
}