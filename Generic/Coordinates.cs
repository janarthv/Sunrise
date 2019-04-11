using System;
using System.Collections.Generic;
using System.Text;

using MathNet.Numerics;

using Sunrise.Generic.Frames;
using Sunrise.CelestialObjects;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;

namespace Sunrise.Generic
{
    public enum CoordinateType
    {
        Keplerian,
        Cartesian,
        BodyCentric,
        TopoCentric,
    }

    public abstract class GenericProperty
    {
        public bool IsValid { get; set; }
    }

    public abstract class BasicCoordinates : GenericProperty
    {
        public Frame? CoordinateFrame { get; set; }
        public Body? Origin { get; set; }
    }

    public enum KeplerianDepth
    {
        PlaneOnly,
        Exact,
    }

    public class KeplerianCoordinates : BasicCoordinates
    {
        public double SMA { get; set; }
        public double Ecc { get; set; }
        public double Inc { get; set; }
        public double RAAN { get; set; }
        public double ArgPer { get; set; }
        public double TA { get; set; }
        public KeplerianDepth Depth { get; set; }

        public KeplerianCoordinates()
        {
        }

        public KeplerianCoordinates(double sma, double ecc, double inc, double raan, double argPer, double ta, Frame frame, Body origin)
        {
            SMA = sma;
            Ecc = ecc;
            Inc = inc;
            RAAN = raan;
            ArgPer = argPer;
            TA = ta;
            CoordinateFrame = frame;
            Origin = origin;
        }

        internal void CheckValidity()
        {
            throw new NotImplementedException();
        }
    }

    public enum CartesianDepth
    {
        Position,
        Velocity,
        Acceleration,
    }

    public class CartesianCoordinates : BasicCoordinates
    {
        public CartesianDepth Depth { get; set; }
        private Vector<double> _pos;
        public Vector<double> Position
        {
            get
            {
                return _pos;
            }
            set
            {
                if (value.Count() != 3)
                {
                    throw new ArgumentException("Position vector should a vector double of length 3");
                }
                else
                {
                    _pos = value;
                }
            }
        }
        private Vector<double> _vel;
        public Vector<double> Velocity
        {
            get
            {
                return _vel;
            }
            set
            {
                if (value.Count() != 3)
                {
                    throw new ArgumentException("Position vector should a vector double of length 3");
                }
                else
                {
                    _vel = value;
                }
            }
        }

        public CartesianCoordinates()
        {

        }

        public CartesianCoordinates(Vector<double> position, Vector<double> velocity, Frame frame, Body origin, CartesianDepth depth)
        {
            CoordinateFrame = frame;
            Origin = origin;
            Position = position;
            Velocity = velocity;
            Depth = depth;
        }

        internal void CheckValidity()
        {
            throw new NotImplementedException();
        }

        public void Negative()
        {
            if (_pos != null)
            {
                _pos = -_pos;
            }
            if (_vel != null)
            {
                _vel = -_vel;
            }
        }
    }

    public class LatLon
    {
        double Lat { get; set; }
        double Lon { get; set; }

        public LatLon()
        {
            Lat = 0D;
            Lon = 0D;
        }

        public LatLon(double lat, double lon)
        {
            Lat = lat;
            Lon = lon;
        }
    }

    public abstract class BodyCoordinates : GenericProperty
    {
        public Body? CentreBody { get; set; }
        public Frame? BodyFrame { get; set; }
    }

    public class BodyCentricCoordinates : BodyCoordinates
    {
        public LatLon LatLon { get; set; }
        public double Altitude { get; set; }
        public BodyCentricCoordinates()
        {
        }

        public BodyCentricCoordinates(LatLon latLon, double altitude, Body body, Frame bodyFrame)
        {
            CentreBody = body;
            BodyFrame = bodyFrame;
            LatLon = latLon;
            Altitude = altitude;
        }

        internal void CheckValidity()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class LocalCoordinates : GenericProperty
    {
        public Frame? LocalFrame { get; set; }
    }

    public class TopoCentricCoordinates : LocalCoordinates
    {
        public BodyCentricCoordinates Location { get; set; }
        public double Azimuth { get; set; }
        public double Elevation { get; set; }
        public double Range { get; set; }

        public TopoCentricCoordinates()
        {
        }

        public TopoCentricCoordinates(BodyCentricCoordinates location, double azimuth, double elevation, double range, Frame localFrame)
        {
            LocalFrame = localFrame;
            Location = location;
            Azimuth = azimuth;
            Elevation = elevation;
            Range = range;
        }

        internal void CheckValidity()
        {
            throw new NotImplementedException();
        }
    }

    public static class CoordinateTransformations
    {
        internal static void ConvertKeplerianToCartesian(KeplerianCoordinates keplerianCoordinates, CartesianCoordinates cartesianCoordinates)
        {
            try
            {
                CheckKeplerianCoordinatesValidity(keplerianCoordinates);
                CheckCartesianCoordinatesValidity(cartesianCoordinates);
            }
            catch
            {
                throw new InvalidOperationException();
            }
            if (keplerianCoordinates.CoordinateFrame != cartesianCoordinates.CoordinateFrame || keplerianCoordinates.Origin == null)
            {
                throw new InvalidOperationException();
            }

            //FIXME
            cartesianCoordinates = new CartesianCoordinates
            {
                Origin = keplerianCoordinates.Origin,
                CoordinateFrame = keplerianCoordinates.CoordinateFrame,
                Position = Vector<double>.Build.Random(3),
                Velocity = Vector<double>.Build.Random(3),
            };
        }

        private static void CheckCartesianCoordinatesValidity(CartesianCoordinates cartesianCoordinates)
        {
            if (cartesianCoordinates == null || cartesianCoordinates.CoordinateFrame == null)
            {
                throw new ArgumentNullException();
            }
        }

        private static void CheckKeplerianCoordinatesValidity(KeplerianCoordinates keplerianCoordinates)
        {
            if (keplerianCoordinates == null || keplerianCoordinates.CoordinateFrame == null)
            {
                throw new ArgumentNullException();
            }
        }
    }

    //public class Coordinates
    //{
    //    public Body? Body { get; set; }
    //    public KeplerianCoordinates KeplerianCoordinates { get; set; }
    //    public IEnumerable<CartesianCoordinates> CartesianCoordinates { get; set; }
    //    public BodyCentricCoordinates BodyCentricCoordinates { get; set; }
    //    public IEnumerable<TopoCentricCoordinates> TopoCentricCoordinates { get; set; }

    //    //internal void ConvertKeplerianToCartesian()
    //    //{
    //    //    try
    //    //    {
    //    //        CheckKeplerianCoordinatesValidity();
    //    //        CheckCartesianCoordinatesValidity();
    //    //    }
    //    //    catch
    //    //    {
    //    //        throw new InvalidOperationException();
    //    //    }
    //    //    if (KeplerianCoordinates.CoordinateFrame != CartesianCoordinates.CoordinateFrame || KeplerianCoordinates.Origin == null)
    //    //    {
    //    //        throw new InvalidOperationException();
    //    //    }

    //    //    //FIXME
    //    //    CartesianCoordinates = new CartesianCoordinates
    //    //    {
    //    //        Origin = KeplerianCoordinates.Origin,
    //    //        CoordinateFrame = KeplerianCoordinates.CoordinateFrame,
    //    //        Position = Vector<double>.Build.Random(3),
    //    //        Velocity = Vector<double>.Build.Random(3),
    //    //    };
    //    //}

    //    //private void CheckCartesianCoordinatesValidity()
    //    //{
    //    //    if (CartesianCoordinates == null || CartesianCoordinates.CoordinateFrame == null)
    //    //    {
    //    //        throw new ArgumentNullException();
    //    //    }
    //    //}

    //    //private void CheckKeplerianCoordinatesValidity()
    //    //{
    //    //    if (KeplerianCoordinates == null || KeplerianCoordinates.CoordinateFrame == null)
    //    //    {
    //    //        throw new ArgumentNullException();
    //    //    }
    //    //}

    //    //public void CheckValidity()
    //    //{
    //    //    string errorMessage = "Coordinates.CheckValidity():";
    //    //    if (Type == CoordinateType.Keplerian)
    //    //    {
    //    //        try
    //    //        {
    //    //            KeplerianCoordinates.CheckValidity();
    //    //        }
    //    //        catch
    //    //        {
    //    //            errorMessage += "Invalid Keplerian coordinates";
    //    //            throw new ArgumentException(errorMessage);
    //    //        }
    //    //    }
    //    //    else if (Type == CoordinateType.Cartesian)
    //    //    {
    //    //        try
    //    //        {
    //    //            CartesianCoordinates.CheckValidity();
    //    //        }
    //    //        catch
    //    //        {
    //    //            errorMessage += "Invalid Cartesian coordinates";
    //    //            throw new ArgumentException(errorMessage);
    //    //        }
    //    //    }
    //    //    else if (Type == CoordinateType.BodyCentric)
    //    //    {
    //    //        try
    //    //        {
    //    //            BodyCentricCoordinates.CheckValidity();
    //    //        }
    //    //        catch
    //    //        {
    //    //            errorMessage += "Invalid BodyCentric coordinates";
    //    //            throw new ArgumentException(errorMessage);
    //    //        }
    //    //    }
    //    //    else if (Type == CoordinateType.TopoCentric)
    //    //    {
    //    //        try
    //    //        {
    //    //            TopoCentricCoordinates.CheckValidity();
    //    //        }
    //    //        catch
    //    //        {
    //    //            errorMessage += "Invalid TopoCentric coordinates";
    //    //            throw new ArgumentException(errorMessage);
    //    //        }
    //    //    }
    //    //}
    //}
}
