using System;
using System.Collections.Generic;
using System.Text;

using MathNet.Numerics;

namespace Sunrise.Math
{
    public static class BasicMath
    {
        public static double RadianToDegree(double radian)
        {
            return Trig.RadianToDegree(radian);
        }

        public static double DegreeToRadian(double degree)
        {
            return Trig.DegreeToRadian(degree);
        }
    }
}
