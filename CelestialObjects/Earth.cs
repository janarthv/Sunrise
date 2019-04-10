using System;
using MathNet.Numerics.LinearAlgebra;
using Sunrise.Generic;
using Sunrise.Generic.Frames;
using Sunrise.Generic.States;
using Sunrise.Math;

namespace Sunrise.CelestialObjects
{
    public static class Earth
    {
        public static void GetHelioCentricKeplerianElements(State state)
        {
            state.CheckValidity();
            KeplerianCoordinates keplerianCoordinates = state.Coordinates.KeplerianCoordinates;
            if (keplerianCoordinates == null || keplerianCoordinates.CoordinateFrame == null)
            {
                throw new ArgumentNullException();
            }
            if (keplerianCoordinates.CoordinateFrame == Frame.EME2000)
            {
                state.Coordinates.KeplerianCoordinates = new KeplerianCoordinates
                {
                    SMA = 1.00000011 * Constants.AU,
                    Ecc = 0.01671022,
                    Inc = BasicMath.DegreeToRadian(0.00005),
                    RAAN = BasicMath.DegreeToRadian(-11.26064),
                    ArgPer = BasicMath.DegreeToRadian(102.94719),
                    TA = BasicMath.DegreeToRadian(100.46435),
                    Origin = Body.Sun,
                    CoordinateFrame = Frame.EME2000,
                };
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}

