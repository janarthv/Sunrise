using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Sunrise.CelestialObjects;

namespace Sunrise.Generic.States
{
    public enum Depth
    {
        Position,
        Velocity,
        Acceleration,
    }

    public static partial class StateRetriever
    {
        private static Dictionary<Body, DefaultState> defaultStates = new Dictionary<Body, DefaultState>();
         
        public static void GetState(State state)
        {
            if (state == null)
            {
                throw new ArgumentNullException();
            }
            state.CheckValidity();

            Body body = state.Body.Value;
            DateTime epoch = state.Epoch;
            DefaultState bodyDefaultState = CelestialBodies.GetDefaultState(body,epoch);
            defaultStates.Add(body, bodyDefaultState);

            foreach (Coordinates mainCoordinates in state.CoordinatesSet)
            {
                if (mainCoordinates.CoordinatesNeeded == null || mainCoordinates.Body == null)
                {
                    throw new ArgumentNullException();
                }

                Body origin = mainCoordinates.Body.Value;
                if (origin == body)
                {
                    throw new InvalidOperationException();
                }

                DefaultState originDefaultState = new DefaultState();
                if (bodyDefaultState.Coordinates.Body != origin)
                {
                    originDefaultState = CelestialBodies.GetDefaultState(origin);
                    defaultStates.Add(origin, originDefaultState);
                }

                CoordinatesNeeded coordinatesNeeded = mainCoordinates.CoordinatesNeeded;

                if (coordinatesNeeded.Keplerian)
                {
                    //Earth-Sun, Sun-Earth
                    if (bodyDefaultState.Coordinates.Body == origin)
                    {
                        if (bodyDefaultState.CoordinateType == CoordinateType.Keplerian)
                        {
                            CoordinateTransformations.ConvertKeplerianFrame(bodyDefaultState.Coordinates.KeplerianCoordinates, mainCoordinates.KeplerianCoordinates);
                        }
                        else if (bodyDefaultState.CoordinateType == CoordinateType.Cartesian)
                        {
                            CoordinateTransformations.ConvertCartesianToKeplerian(bodyDefaultState.Coordinates.CartesianCoordinates.First(),mainCoordinates.KeplerianCoordinates);
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                    // Sun-Mars , Sun default earth, Mars default sun
                    else if (originDefaultState.Coordinates.Body == body)
                    {
                        CartesianCoordinates dummyCartesianCoordinates = new CartesianCoordinates
                        {
                            CoordinateFrame = mainCoordinates.KeplerianCoordinates.CoordinateFrame,
                            Depth = CartesianDepth.Velocity,
                        };
                        if (originDefaultState.CoordinateType == CoordinateType.Keplerian)
                        {
                            CartesianCoordinates cartesianCoordinates1 = new CartesianCoordinates
                            {
                                Depth = CartesianDepth.Velocity,
                            };
                            CoordinateTransformations.ConvertKeplerianToCartesian(originDefaultState.Coordinates.KeplerianCoordinates, cartesianCoordinates1);

                            CoordinateTransformations.ConvertCartesianFrame(cartesianCoordinates1, dummyCartesianCoordinates);                           
                        }
                        else if (originDefaultState.CoordinateType == CoordinateType.Cartesian)
                        {
                            CoordinateTransformations.ConvertCartesianFrame(originDefaultState.Coordinates.CartesianCoordinates.First(), dummyCartesianCoordinates);
                        }
                        dummyCartesianCoordinates.Negative();
                        CoordinateTransformations.ConvertCartesianToKeplerian(dummyCartesianCoordinates,mainCoordinates.KeplerianCoordinates);
                    }

                    //Moon-Sun
                    //Sun-Moon
                }
                if (coordinatesNeeded.Cartesian)
                {
                    foreach (CartesianCoordinates cartesianCoordinates in mainCoordinates.CartesianCoordinates)
                    {
                        if (cartesianCoordinates == null || cartesianCoordinates.CoordinateFrame == null)
                        {
                            throw new ArgumentNullException();
                        }
                        cartesianCoordinates.Origin = origin;
                        if (origin != body)
                        {
                            CartesianCoordinates dummyCartesianCoordinates = new CartesianCoordinates
                            {
                                Depth = cartesianCoordinates.Depth,
                                CoordinateFrame = cartesianCoordinates.CoordinateFrame,
                            };
                            Coordinates dummyCoordinates = new Coordinates
                            {
                                Body = origin,
                                CoordinatesNeeded = new CoordinatesNeeded
                                {
                                    Cartesian = true,
                                },
                                KeplerianCoordinates = mainCoordinates.KeplerianCoordinates,
                                CartesianCoordinates = new List<CartesianCoordinates>
                                        {
                                            dummyCartesianCoordinates,
                                        }
                            };
                            GetHelioCentricState(state.Epoch, dummyCoordinates);
                            dummyCartesianCoordinates.Negative(); //BETTERME
                            cartesianCoordinates.Position = dummyCartesianCoordinates.Position;
                            if (cartesianCoordinates.Depth >= CartesianDepth.Velocity)
                            {
                                cartesianCoordinates.Velocity = dummyCartesianCoordinates.Velocity;
                            }
                        }
                        else
                        {
                            cartesianCoordinates.Position = Vector<double>.Build.Dense(3);
                            if (cartesianCoordinates.Depth >= CartesianDepth.Velocity)
                            {
                                cartesianCoordinates.Velocity = Vector<double>.Build.Dense(3);
                            }
                        }
                    }
                }
            }                
        }
    }
}
