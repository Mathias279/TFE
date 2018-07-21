using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Abstract;

namespace TFE2017.Core.Models
{
    class RoomEntity : PlaceEntity
    {
        public override double Distance { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override double Accessibility { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PositionEntity Coordonates { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RoomEntity()
        {
        }
    }
}
