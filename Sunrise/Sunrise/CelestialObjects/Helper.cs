using System;
using Sunrise.Generic;

namespace Sunrise.CelestialObjects
{
    public static class Helper
    {
        public static void GetState(Body body, State state)
        {
            state.CheckValidity();
            if (body == Body.Sun)
            {
                if (state.Coordinates.Type == CoordinateType.Cartesian)
                {
                    Body origin = state.Coordinates.CartesianCoordinates.Origin;
                    if (origin == Body.Sun)
                    {
                        state.Coordinates.CartesianCoordinates = new CartesianCoordinates
                        {
                            Position = new Position(),
                            Velocity = new Velocity(),
                        };
                    }
                    else
                    {
                        //FIXME  
                        state = new State
                        {
                            Epoch = state.Epoch,
                            Coordinates = new Coordinates
                            {
                                Type = CoordinateType.Cartesian,
                                CartesianCoordinates = new CartesianCoordinates
                                {
                                    Frame = state.Coordinates.CartesianCoordinates.Frame,
                                },
                            },
                        };
                        GetHelioCentricState(origin,state);
                    }
                }
            }
        }

        private static void GetHelioCentricState(Body body, State state)
        {
            if (body == Body.Earth)
            {
                Earth.GetHelioCentricState(state);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

    }
}
