using System;
using System.Collections.Generic;
using System.Text;
using TFE2017.Core.Models;

namespace TFE2017.Core.Managers
{
    static class MapManager
    {

        /// <summary>
        /// Returns the distance between two positions
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static public double GetDistance(PositionEntity a, PositionEntity b)
        {
            double dX = a.X - b.X;
            double dY = a.Y - b.Y;
            double dZ = a.Z - b.Z;
            
            return Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2) + Math.Pow(dZ, 2));
        }

        /// <summary>
        /// Returns the direction in dgrees from first to second (returns a negative is same position
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        static public int GetDirection(PositionEntity start, PositionEntity finish)
        {
            if (start == finish)
            {
                return -1;
            }
            else
            {
                double dX = finish.X - start.X;
                double dY = finish.Y - start.Y;

                //pas Est pas Ouest 
                if (dX == 0)
                {
                    //Nord
                    if (dY > 0)
                        return 0;
                    //Sud
                    else
                        return 180;
                }
                //pas Nord pas Sud 
                else if (dY == 0)
                {
                    //Est
                    if (dX > 0)
                        return 90;
                    //Ouest
                    else
                        return 270;
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

    }
}
