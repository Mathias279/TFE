using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models
{
    class UserEntity
    {
        public PositionEntity Position { get; set; }
        public string Name { get; private set; }
        public double Orientation { get; set; }

    }
}
