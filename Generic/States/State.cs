using MathNet.Numerics.LinearAlgebra;
using Sunrise.CelestialObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sunrise.Generic.States
{
    /*
     * Each set of coordinates is associated with a body.
     * Eg: For a satellite, one could think of a set of coordinates for body earth 
     * 1. Keplerian
     * 2. List of Cartesian, 
     *      1. Inertial frame
     *      2. Body fixed frame
     * 3. Body-centric coordinates for lat, lon
     * 4. List of topocentric
     *      1. With respect to several ground locations
     */

    /* Concept of state
     * 1. Epoch
     * 2. List of coordinates
     *      1. Satellite w.r.t sun
     *      2. W.r.t earth
     */

    /*
     * Concept of state retriever
     * 1. Body is satellite
     * 2. Coordinates needed is meant for all coordinates or could be object specific (meaning you might want only cartesian state 
     *    w.r.t sun but also other coordinates w.r.t earth)
     *    If higher lever coordinates needed is false none of the lower coordinates will be created.
     *    If higher level is true and lower level is false, then for that object (for eg topocentric for satellite w.r.t sun) those coordinates will not be computed
     */

    public class CoordinatesNeeded
    {
        public bool Keplerian { get; set; }
        public bool Cartesian { get; set; }
        public bool BodyCentric { get; set; }
        public bool TopoCentric { get; set; }
    }

    public class Coordinates
    {
        public Body? Body { get; set; }
        public CoordinatesNeeded CoordinatesNeeded { get; set; }
        public KeplerianCoordinates KeplerianCoordinates { get; set; }
        public IEnumerable<CartesianCoordinates> CartesianCoordinates { get; set; }
        public BodyCentricCoordinates BodyCentricCoordinates { get; set; }
        public IEnumerable<TopoCentricCoordinates> TopoCentricCoordinates { get; set; }
    }

    public class State
    {
        public Body? Body { get; set; }
        public DateTime Epoch { get; set; }
        public IEnumerable<Coordinates> CoordinatesSet { get; set; }

        public void CheckValidity()
        {
            string errorMessage = "State.CheckValidity():";

            if (Body == null)
            {
                errorMessage += "State body";
                throw new ArgumentNullException(errorMessage);
            }

            if (Epoch == null)
            {
                errorMessage += "State epoch";
                throw new ArgumentNullException(errorMessage);
            }

            if (CoordinatesSet == null || !CoordinatesSet.Any() || CoordinatesSet.Any( x => x == null || x.Body == null))
            {
                errorMessage += "State coordinates";
                throw new ArgumentNullException("State coordinates");
            }

            IEnumerable<Body> bodies = CoordinatesSet.Select(x => x.Body.Value);
            if (bodies.Distinct().Count() != bodies.Count())
            {
                throw new ArgumentException("Coordinates set has 2 coordinates associated with same body ");
            }
        }
    }
}
