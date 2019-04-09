using System;

using Sunrise.Generic.Frames;
using Sunrise.CelestialObjects;

namespace Sunrise.Generic
{
    public class State
    {
        public DateTime Epoch { get; set; }
        public Coordinates Coordinates { get; set; }
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
            try
            {
                Coordinates.CheckValidity();
            }
            catch 
            {
                errorMessage += "Invalid state coordinates";
                throw new ArgumentException(errorMessage);
            }
        }
    }
}
