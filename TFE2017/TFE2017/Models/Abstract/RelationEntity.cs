using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models.Abstract
{
    abstract class RelationEntity:IPlaceEntity  
    {
        public PositionEntity Position { get; set; }
    }
}