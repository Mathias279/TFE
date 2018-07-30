using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Abstract;

namespace TFE2017.Core.Models
{
    class DoorEntity: RelationEntity
    {
        public DoorEntity(string id, string name, PositionEntity position, double accessibilite, double width)
        {
            Id = id;
            Name = name;
            Coordonates = position;
            Accessibility = accessibilite;
            Width = width;
        }

        private void SetCenter()
        {
            //Center = new PositionEntity((Edge1.X + Edge2.X) / 2, (Edge1.Y + Edge2.Y) / 2);
        }
        private void SetWidth()
        {
            //double diffX = Edge1.X - Edge2.X;
            //double diffY = Edge1.Y - Edge2.Y;
            //Width = Math.Sqrt(Math.Pow(diffX, (double)2) + Math.Pow(diffY, (double)2));
        }
    }
}
