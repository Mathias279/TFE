using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Abstract;

namespace TFE2017.Core.Models
{
    class Elevation : RelationEntity
    {
        public Elevation()
        {
            Position = new PositionEntity(0, 0, 0);
        }
    }
}
