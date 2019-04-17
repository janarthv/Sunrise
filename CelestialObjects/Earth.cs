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
        public static DefaultState GetDefaultState(DateTime epoch)
        {
            KeplerianCoordinates keplerianCoordinates = new KeplerianCoordinates
            {
                CoordinateFrame = Frame.EME2000,
                Depth = KeplerianDepth.Exact,
            };
            GetHelioCentricKeplerianElements(epoch, keplerianCoordinates);
            return new DefaultState
            {
                Epoch = epoch,
                CoordinateType = CoordinateType.Keplerian,
                KeplerianCoordinates = keplerianCoordinates,
            };
        }
        public static void GetHelioCentricKeplerianElements(DateTime epoch, KeplerianCoordinates keplerianCoordinates)
        {            
            if (keplerianCoordinates == null || keplerianCoordinates.CoordinateFrame == null)
            {
                throw new ArgumentNullException();
            }
            if (keplerianCoordinates.CoordinateFrame == Frame.EME2000)
            {
                KeplerianCoordinates dummy = new KeplerianCoordinates
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
                keplerianCoordinates.SMA = dummy.SMA;
                keplerianCoordinates.Ecc = dummy.Ecc;
                keplerianCoordinates.Inc = dummy.Inc;
                keplerianCoordinates.RAAN = dummy.RAAN;
                keplerianCoordinates.ArgPer = dummy.ArgPer;
                keplerianCoordinates.TA = dummy.TA;
                keplerianCoordinates.Origin = dummy.Origin;
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}

