﻿using System;

using Sunrise.CelestialObjects;

namespace Sunrise.Generic.States
{
    public partial class StateRetriever
    {
        public static void GetHelioCentricState(Body body, DateTime epoch, Coordinates coordinates)
        {
            /*
             * Possible use would be : get for a satellite several heliocentric states in different frames, though it is doubtful one would ever need this.
             * Normally, you would expect to retrieve one set of Keplerian and Cartesian state with same frame
             */
            KeplerianCoordinates keplerianCoordinates = coordinates.KeplerianCoordinates;
            if (coordinates.CoordinatesNeeded.Keplerian)
            {
                GetHelioCentricKeplerianElements(body, epoch, ref keplerianCoordinates);
            }

            if (coordinates.CoordinatesNeeded.Cartesian)
            {
                KeplerianCoordinates dummyKeplerianCoordinates;
                foreach (CartesianCoordinates cartesianCoordinates in coordinates.CartesianCoordinates)
                {
                    if (keplerianCoordinates == null || keplerianCoordinates.CoordinateFrame != cartesianCoordinates.CoordinateFrame)//TESTME sending a nonnull KeplerianCoord with a null coordinate frame
                    {
                        dummyKeplerianCoordinates = new KeplerianCoordinates
                        {
                            CoordinateFrame = cartesianCoordinates.CoordinateFrame
                        };
                        GetHelioCentricKeplerianElements(body,epoch, ref dummyKeplerianCoordinates);
                    }
                    else
                    {
                        dummyKeplerianCoordinates = keplerianCoordinates;
                    }
                    CoordinateTransformations.ConvertKeplerianToCartesian(dummyKeplerianCoordinates,cartesianCoordinates);
                }
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static void GetHelioCentricKeplerianElements(Body body, DateTime epoch, ref KeplerianCoordinates keplerianCoordinates)
        {
            if (body == CelestialObjects.Body.Sun)
            {
                throw new InvalidOperationException();
            }
            if (body == CelestialObjects.Body.Earth)
            {
                Earth.GetHelioCentricKeplerianElements(epoch, ref keplerianCoordinates);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
