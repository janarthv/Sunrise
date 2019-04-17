using System;
using MathNet.Numerics.LinearAlgebra;
using Sunrise.Generic;
using Sunrise.Generic.Frames;
using Sunrise.Generic.States;
using Sunrise.Math;

namespace Sunrise.CelestialObjects
{
    public enum CoordinateType
    {
        Cartesian,
        Keplerian,
    }
    public class DefaultState
    {
        public DateTime Epoch { get; set; }
        public CoordinateType CoordinateType { get; set; }
        public KeplerianCoordinates KeplerianCoordinates { get; set; }
        public CartesianCoordinates CartesianCoordinates { get; set; }
    }
    public static class Sun
    {
        public static DefaultState GetDefaultState(DateTime epoch)
        {
            DefaultState earthDefaultState = Earth.GetDefaultState(epoch);
            earthDefaultState.CartesianCoordinates = new CartesianCoordinates();
            CoordinateTransformations.ConvertKeplerianToCartesian(earthDefaultState.KeplerianCoordinates, earthDefaultState.CartesianCoordinates);
            earthDefaultState.CartesianCoordinates.Negative();
            CartesianCoordinates cartesianCoordinates = earthDefaultState.CartesianCoordinates;
            cartesianCoordinates.Origin = Body.Earth;

            return new DefaultState
            {
                Epoch = epoch,
                CoordinateType = CoordinateType.Cartesian,
                CartesianCoordinates = cartesianCoordinates,
            };
        }
    }
}

