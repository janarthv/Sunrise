using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;
using Sunrise.Generic;
using Sunrise.Generic.Frames;
using Sunrise.Generic.States;
using Sunrise.Math;

namespace Sunrise.CelestialObjects
{
    public class DefaultState
    {
        public DateTime? Epoch { get; set; }
        public CoordinateType CoordinateType { get; set; }
        public Coordinates Coordinates { get; set; }
    }

    public static class Sun
    {
        public static DefaultState GetDefaultState(DateTime? epoch = null)
        {
            CartesianCoordinates cartesianCoordinates = new CartesianCoordinates
            {
                CoordinateFrame = Frame.EME2000,
                Origin = Body.Earth,
            };
            Coordinates coordinates = new Coordinates
            {
                Body = Body.Earth,
                CartesianCoordinates = new List<CartesianCoordinates>
                {
                    cartesianCoordinates,
                },
            };
            DefaultState defaultState = new DefaultState
            {
                CoordinateType = CoordinateType.Cartesian,
                Coordinates = coordinates,
            };
            if (epoch == null)
            {
                return defaultState; //FIXME
            }
            else
            {
                defaultState.Epoch = epoch;
                DefaultState earthDefaultState = Earth.GetDefaultState(epoch);
                CartesianCoordinates earthCartesian = new CartesianCoordinates
                {
                    Depth = CartesianDepth.Velocity,
                };
                CoordinateTransformations.ConvertKeplerianToCartesian(earthDefaultState.Coordinates.KeplerianCoordinates, earthCartesian);
                earthCartesian.Negative();
                cartesianCoordinates = earthCartesian;
                cartesianCoordinates.Origin = Body.Earth;
                defaultState.Coordinates.CartesianCoordinates = new List<CartesianCoordinates>
                {
                    cartesianCoordinates
                };
                return defaultState;
            }
        }
    }
}

