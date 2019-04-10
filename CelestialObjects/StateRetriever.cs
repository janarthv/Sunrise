using System;
using MathNet.Numerics.LinearAlgebra;
using Sunrise.Generic;

namespace Sunrise.CelestialObjects
{
    public enum Depth
    {
        Position,
        Velocity,
        Acceleration,
    }

    public class CoordinatesNeeded
    {
        public bool Keplerian { get; set; }
        public bool Cartesian { get; set; }
        public bool BodyCentric { get; set; }
        public bool TopoCentric { get; set; }
    }

    public class StateRetriever
    {
        public State State { get; set; }
        public CoordinatesNeeded CoordinatesNeeded { get; set; }
    }
}
