using System;
using Sunrise;
using Sunrise.CelestialObjects;

namespace Sunrise.Astronomy
{
    public static class Settings
    {
        public static void SetEarthBased()
        {
            Sunrise.Settings.ReferenceOrigin = Origin.Earth;
        }
    }
}
