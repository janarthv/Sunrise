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
        public static DefaultState GetDefaultState(DateTime? epoch = null)
        {
            KeplerianCoordinates keplerianCoordinates = new KeplerianCoordinates
            {
                CoordinateFrame = Frame.EME2000,
                Origin = Body.Sun,
                Depth = KeplerianDepth.Exact,
            };
            Coordinates coordinates = new Coordinates
            {
                Body = Body.Sun,
                KeplerianCoordinates = keplerianCoordinates,
            };
            DefaultState defaultState = new DefaultState
            {
                CoordinateType = CoordinateType.Keplerian,
                Coordinates = coordinates,
            };
            if (epoch == null)
            {
                return defaultState; //FIXME throw exception
            }
            else
            {
                GetHelioCentricKeplerianElements(epoch.Value, keplerianCoordinates);
                defaultState.Epoch = epoch.Value;
                return defaultState;
            }
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
                    TrueAnom = BasicMath.DegreeToRadian(100.46435),
                    Origin = Body.Sun,
                    CoordinateFrame = Frame.EME2000,
                };

                keplerianCoordinates.Origin = dummy.Origin;
                keplerianCoordinates.IsValid = true;
                keplerianCoordinates.SMA = dummy.SMA;
                keplerianCoordinates.Ecc = dummy.Ecc;
                keplerianCoordinates.Inc = dummy.Inc;
                keplerianCoordinates.RAAN = dummy.RAAN;

                if (keplerianCoordinates.Depth > KeplerianDepth.PlaneOnly)
                {
                    keplerianCoordinates.ArgPer = dummy.ArgPer;
                    keplerianCoordinates.TrueAnom = dummy.TrueAnom;
                }

            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}

