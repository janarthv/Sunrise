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

    public interface IGenericDefaultMethods
    {
        void CheckValidity(ValidityDepth? validityDepth = null);
    }

    public interface ISpecificDefaultMethods
    {
        void CheckCoordinates();
    }

    public abstract class GenericProperty
    {
        public bool IsValid { get; set; }
    }

    public enum ValidityDepth
    {
        NullCheck,
        Origin,
        CoordinateFrame,
        CentreBody,
        BodyFrame,
    }

    public abstract class BasicCoordinates : GenericProperty, IGenericDefaultMethods
    {
        public Frame? CoordinateFrame { get; set; }
        public Body? Origin { get; set; }

        public void CheckValidity(ValidityDepth? validityDepth = null)
        {
            if (validityDepth == null)
            {
                validityDepth = ValidityDepth.NullCheck;
            }

            if (validityDepth <= ValidityDepth.NullCheck)
            {
                if (this == null)
                {
                    throw new ArgumentNullException();
                }
            }
            else if (validityDepth <= ValidityDepth.Origin)
            {
                if (this == null)
                {
                    throw new ArgumentNullException();
                }

                if (Origin == null)
                {
                    throw new ArgumentNullException();
                }
            }
            else if (validityDepth <= ValidityDepth.CoordinateFrame)
            {
                if (this == null)
                {
                    throw new ArgumentNullException();
                }

                if (Origin == null)
                {
                    throw new ArgumentNullException();
                }
                if (CoordinateFrame == null)
                {
                    throw new ArgumentNullException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public enum KeplerianDepth
    {
        PlaneOnly,
        Exact,
    }

    public class KeplerianCoordinates : BasicCoordinates, ISpecificDefaultMethods
    {
        public double SMA { get; set; }
        public double Ecc { get; set; }
        public double Inc { get; set; }
        public double RAAN { get; set; }
        public double ArgPer { get; set; }
        public double TrueAnom { get; set; }
        public KeplerianDepth Depth { get; set; }

        public KeplerianCoordinates()
        {
        }

        public KeplerianCoordinates(double sma, double ecc, double inc, double raan, double argPer, double trueAnom, Frame frame, Body origin)
        {
            SMA = sma;
            Ecc = ecc;
            Inc = inc;
            RAAN = raan;
            ArgPer = argPer;
            TrueAnom = trueAnom;
            CoordinateFrame = frame;
            Origin = origin;
        }

        public void CheckCoordinates()
        {
            CheckValidity(ValidityDepth.CoordinateFrame);
            if (IsValid != true || new List<double> { SMA, Ecc, Inc, RAAN }.Any(x => x == default(double)))
            {
                throw new ArgumentNullException("Coordinates are not valid");
            }

            if (Depth > KeplerianDepth.PlaneOnly)
            {
                if (new List<double> { ArgPer, TrueAnom }.Any(x => x == default(double)))
                {
                    throw new ArgumentNullException("Coordinates are not valid");
                }
            }
        }
    }

    public enum CartesianDepth
    {
        Position,
        Velocity,
        Acceleration,
    }

    public class CartesianCoordinates : BasicCoordinates, ISpecificDefaultMethods
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
                    throw new ArgumentException("Velocity vector should a vector double of length 3");
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

        public void CheckCoordinates()
        {
            CheckValidity(ValidityDepth.CoordinateFrame);
            if (IsValid != true || Position == null)
            {
                throw new ArgumentNullException("Coordinates are not valid");
            }

            if (Depth > CartesianDepth.Position)
            {
                if (Velocity == null)
                {
                    throw new ArgumentNullException("Coordinates are not valid");
                }
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

    public abstract class BodyCoordinates : GenericProperty, IGenericDefaultMethods
    {
        public Body? CentreBody { get; set; }
        public Frame? BodyFrame { get; set; }

        public void CheckValidity(ValidityDepth? validityDepth = null)
        {
            throw new NotImplementedException();
        }
    }

    public class BodyCentricCoordinates : BodyCoordinates, ISpecificDefaultMethods
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

        public void CheckCoordinates()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class LocalCoordinates : GenericProperty, IGenericDefaultMethods
    {
        public Frame? LocalFrame { get; set; }

        public void CheckValidity(ValidityDepth? validityDepth = null)
        {
            throw new NotImplementedException();
        }
    }

    public class TopoCentricCoordinates : LocalCoordinates, ISpecificDefaultMethods
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

        public void CheckCoordinates()
        {
            throw new NotImplementedException();
        }
    }

    public static class CoordinateTransformations
    {
        internal static void ConvertCartesianToKeplerian(CartesianCoordinates cartesianCoordinates, KeplerianCoordinates keplerianCoordinates)
        {
            throw new NotImplementedException();
        }

        internal static void ConvertKeplerianFrame(KeplerianCoordinates keplerianCoordinates1, KeplerianCoordinates keplerianCoordinates2)
        {
            try
            {
                keplerianCoordinates1.CheckCoordinates();
                keplerianCoordinates2.CheckValidity(ValidityDepth.CoordinateFrame);
                if (keplerianCoordinates2.Depth > keplerianCoordinates1.Depth)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }

            keplerianCoordinates2.IsValid = true;

            if (keplerianCoordinates1.CoordinateFrame != keplerianCoordinates2.CoordinateFrame)
            {
                throw new NotImplementedException();
            }
            else
            {
                keplerianCoordinates2.SMA = keplerianCoordinates1.SMA;
                keplerianCoordinates2.Ecc = keplerianCoordinates1.Ecc;
                keplerianCoordinates2.Inc = keplerianCoordinates1.Inc;
                keplerianCoordinates2.RAAN = keplerianCoordinates1.RAAN;
                if (keplerianCoordinates2.Depth > KeplerianDepth.PlaneOnly)
                {
                    keplerianCoordinates2.ArgPer = keplerianCoordinates1.ArgPer;
                    keplerianCoordinates2.TrueAnom = keplerianCoordinates1.TrueAnom;
                }
            }
        }

        internal static void ConvertKeplerianToCartesian(KeplerianCoordinates keplerianCoordinates, CartesianCoordinates cartesianCoordinates)
        {
            try
            {
                keplerianCoordinates.CheckCoordinates();
            }
            catch
            {
                throw new InvalidOperationException();
            }

            //FIXME
            cartesianCoordinates.Origin = keplerianCoordinates.Origin;
            cartesianCoordinates.CoordinateFrame = keplerianCoordinates.CoordinateFrame;

            cartesianCoordinates.Position = Vector<double>.Build.Random(3);
            if (cartesianCoordinates.Depth >= CartesianDepth.Velocity)
            {
                cartesianCoordinates.Velocity = Vector<double>.Build.Random(3);
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
