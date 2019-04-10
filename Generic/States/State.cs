using Sunrise.CelestialObjects;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sunrise.Generic.States
{
    public class State
    {
        public DateTime Epoch { get; set; }
        public Dictionary<Body,Coordinates> CoordinatesSet { get; set; }

        public void CheckValidity()
        {
            string errorMessage = "State.CheckValidity():";
            if (Epoch == null)
            {
                errorMessage += "State epoch";
                throw new ArgumentNullException(errorMessage);
            }
            if (Coordinates == null)
            {
                errorMessage += "State coordinates";
                throw new ArgumentNullException("State coordinates");
            }
        }
    }
}
