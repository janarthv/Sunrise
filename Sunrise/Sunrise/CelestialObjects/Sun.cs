using System;
using Sunrise.Generic;
using Sunrise.Generic.Frames;

namespace Sunrise.CelestialObjects
{
    public class Sun : CelestialBody
    {
        public Sun()
        {
            Name = "Sun";
            MU = Constants.MU.Sun;
        }

        public override void GetState(State state)
        {
            if (state.Origin != Body.Sun)
            {
                
            }


        }

    };
}
