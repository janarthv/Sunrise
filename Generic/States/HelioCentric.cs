using System;

using Sunrise.CelestialObjects;

namespace Sunrise.Generic.States
{
    public partial class StateRetriever
    {
        public void GetHelioCentricState(Depth depth)
        {
            if (Body == CelestialObjects.Body.Sun)
            {
                throw new InvalidOperationException();
            }
            Coordinates coordinates = State.Coordinates;
            if (CoordinatesNeeded.Keplerian)
            {
                GetHelioCentricKeplerianElements();
            }
            if (CoordinatesNeeded.Cartesian)
            {
                CartesianCoordinates cartesianCoordinates = coordinates.CartesianCoordinates;
                KeplerianCoordinates keplerianCoordinates = coordinates.KeplerianCoordinates;

                if (keplerianCoordinates == null || keplerianCoordinates.Origin != CelestialObjects.Body.Sun && keplerianCoordinates.CoordinateFrame != cartesianCoordinates.CoordinateFrame)
                {
                    State intState = new State
                    {
                        Epoch = State.Epoch,
                        Coordinates = new Coordinates
                        {
                            KeplerianCoordinates = new KeplerianCoordinates
                            {
                                CoordinateFrame = cartesianCoordinates.CoordinateFrame,
                            }
                        }
                    };
                    StateRetriever stateRetriever = new StateRetriever
                    {
                        Body = Body,
                        CoordinatesNeeded = new CoordinatesNeeded
                        {
                            Keplerian = true,
                        },
                        State = intState,
                    };
                    stateRetriever.GetHelioCentricKeplerianElements();
                    coordinates.KeplerianCoordinates = intState.Coordinates.KeplerianCoordinates;
                }
                coordinates.ConvertKeplerianToCartesian();
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public void GetHelioCentricKeplerianElements()
        {
            if (Body == CelestialObjects.Body.Sun)
            {
                throw new InvalidOperationException();
            }
            if (Body == CelestialObjects.Body.Earth)
            {
                Earth.GetHelioCentricKeplerianElements(State);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
