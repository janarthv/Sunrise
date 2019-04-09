using System;
using System.Collections.Generic;
using System.Text;

namespace Sunrise.Generic
{
    /*
     * All constants are in units km, kg, secs 
     */
    public static class Constants
    {
        public const double AU = 1.5E8; 

        public static class MU
        {
            public const double Sun = 1.32712440018E11;
            public const double Earth = 3.986004418E5;
        }
    }
}
