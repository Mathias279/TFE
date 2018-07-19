using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models.Interfaces;

namespace TFE2017.Core.Models
{
    class DoorEntity: BuildingEntity
    {
        public PositionEntity Edge1 { get; private set; }
        public PositionEntity Edge2 { get; private set; }
        public PositionEntity Center { get; private set; }
        public double Width { get; private set; }

        public DoorEntity(PositionEntity edge1, PositionEntity edge2)
        {
            Edge1 = edge1;
            Edge2 = edge2;

            SetCenter();
            SetWidth();
        }
        public DoorEntity(double x1, double y1, double x2, double y2)
        {
            new DoorEntity(new PositionEntity(x1, y1), new PositionEntity(x2,y2));
        }

        private void SetCenter()
        {
            Center = new PositionEntity((Edge1.X + Edge2.X) / 2, (Edge1.Y + Edge2.Y) / 2);
        }

        private void SetWidth()
        {
            double diffX = Edge1.X - Edge2.X;
            double diffY = Edge1.Y - Edge2.Y;
            Width = Math.Sqrt(Math.Pow(diffX, (double)2) + Math.Pow(diffY, (double)2));
        }


    }
}
