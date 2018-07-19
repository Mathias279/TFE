using System;
using System.Collections.Generic;
using System.Text;

namespace TFE2017.Core.Models
{
    public class PositionEntity
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public PositionEntity(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double GetDistance(PositionEntity a, PositionEntity b)
        {
            double dX = a.X - b.X;
            double dY = a.Y - b.Y;

            return Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));
        }

        public int GetDirection(PositionEntity begin, PositionEntity end)
        {
            if (begin == end)
            {
                return -1;
            }
            else
            {

                double dX = end.X - begin.X;
                double dY = end.Y - begin.Y;


                //pas Est pas Ouest 
                if (dX == 0)
                {
                    //Nord
                    if (dY > 0)
                    {
                        return 0;
                    }
                    //Sud
                    else
                    {
                        return 180;
                    }
                }
                //pas Nord pas Sud 
                else if (dY == 0)
                {
                    //Est
                    if (dX > 0)
                    {
                        return 90;
                    }
                    //Ouest
                    else
                    {
                        return 270;
                    }
                }
                else
                {
                    double angle = dY / dX;

                    //Nord ...
                    if (dY > 0)
                    {
                        //Est
                        if (dX > 0)
                        {
                            //1 - 89
                            return (int)Math.Round(Math.Abs(angle - 1) * 90);
                        }
                        //Ouest
                        else
                        {
                            //271 - 359
                            return (int)Math.Round((angle + 3) * 90);
                        }
                    }
                    //Sud ...
                    else
                    {
                        //Est
                        if (dX > 0)
                        {
                            //91 - 179
                            return (int)Math.Round(Math.Abs(angle - 1) * 90);
                        }
                        //Ouest
                        else
                        {
                            //181 - 269
                            return (int)Math.Round((angle + 3) * 90);

                        }

                    }
                }
            }
        }

        public new string ToString()
        {
            return string.Join(" ", this.X.ToString(), this.Y.ToString());
        }
    }
}
