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
        void CheckSkeleton(ValidityDepth? validityDepth = null);
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
        Origin,
        CoordinateFrame,
        CentreBody,
        BodyFrame,
    }

    public abstract class BasicCoordinates : GenericProperty, IGenericDefaultMethods
    {
        public Frame? CoordinateFrame { get; set; }
        public Body? Origin { get; set; }

        public void CheckSkeleton(ValidityDepth? validityDepth = null)
        {
            if (this == null)
            {
                throw new ArgumentNullException();
            }

            if (validityDepth == ValidityDepth.Origin)
            {
                if (Origin == null)
                {
                    throw new ArgumentNullException();
                }
            }
            else if (validityDepth == ValidityDepth.CoordinateFrame)
            {
                if (CoordinateFrame == null)
                {
                    throw new ArgumentNullException();
                }
            }
            else if (validityDepth != null)
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
            CheckSkeleton(ValidityDepth.Origin);
            CheckSkeleton(ValidityDepth.CoordinateFrame);

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
            CheckSkeleton(ValidityDepth.Origin);
            CheckSkeleton(ValidityDepth.CoordinateFrame);

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

        public void CheckSkeleton(ValidityDepth? validityDepth = null)
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

        public void CheckSkeleton(ValidityDepth? validityDepth = null)
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
        internal static void ConvertCartesianFrame(CartesianCoordinates cartesianCoordinates1, CartesianCoordinates cartesianCoordinates2)
        {
            try
            {
                cartesianCoordinates1.CheckCoordinates();
                cartesianCoordinates2.CheckSkeleton(ValidityDepth.CoordinateFrame);
                if (cartesianCoordinates2.Depth > cartesianCoordinates1.Depth)
                {
                    throw new ArgumentException();
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }

            cartesianCoordinates2.IsValid = true;
            cartesianCoordinates2.Origin = cartesianCoordinates1.Origin;

            //FIXME
            if (cartesianCoordinates1.CoordinateFrame != cartesianCoordinates2.CoordinateFrame)
            {
                throw new NotImplementedException();
            }
            else
            {
                cartesianCoordinates2.Position = cartesianCoordinates1.Position*10;
                if (cartesianCoordinates2.Depth > CartesianDepth.Velocity)
                {
                    cartesianCoordinates2.Velocity = cartesianCoordinates1.Velocity*10;
                }
            }
        }

        internal static void ConvertCartesianToKeplerian(CartesianCoordinates cartesianCoordinates, KeplerianCoordinates keplerianCoordinates)
        {
            //FIXME
            try
            {
                if (cartesianCoordinates.Depth < CartesianDepth.Velocity)
                {
                    throw new ArgumentException();
                }
                cartesianCoordinates.CheckCoordinates();
            }
            catch
            {
                throw new InvalidOperationException();
            }

            //FIXME
            keplerianCoordinates.Origin = cartesianCoordinates.Origin;
            keplerianCoordinates.CoordinateFrame = cartesianCoordinates.CoordinateFrame;
            keplerianCoordinates.IsValid = true;

            keplerianCoordinates.SMA = 1D;
            keplerianCoordinates.Ecc = 1D;
            keplerianCoordinates.Inc = 1D;
            keplerianCoordinates.RAAN = 1D;

            if (keplerianCoordinates.Depth >= KeplerianDepth.PlaneOnly)
            {
                keplerianCoordinates.ArgPer = 1D;
                keplerianCoordinates.TrueAnom = 1D;
            }
        }

        internal static void ConvertKeplerianFrame(KeplerianCoordinates keplerianCoordinates1, KeplerianCoordinates keplerianCoordinates2)
        {
            try
            {
                keplerianCoordinates1.CheckCoordinates();
                keplerianCoordinates2.CheckSkeleton(ValidityDepth.CoordinateFrame);
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
            keplerianCoordinates2.Origin = keplerianCoordinates1.Origin;

            //FIXME
            if (keplerianCoordinates1.CoordinateFrame != keplerianCoordinates2.CoordinateFrame)
            {
                throw new NotImplementedException();
            }
            else
            {
                keplerianCoordinates2.SMA = keplerianCoordinates1.SMA*10;
                keplerianCoordinates2.Ecc = keplerianCoordinates1.Ecc*10;
                keplerianCoordinates2.Inc = keplerianCoordinates1.Inc * 10;
                keplerianCoordinates2.RAAN = keplerianCoordinates1.RAAN * 10;
                if (keplerianCoordinates2.Depth > KeplerianDepth.PlaneOnly)
                {
                    keplerianCoordinates2.ArgPer = keplerianCoordinates1.ArgPer * 10;
                    keplerianCoordinates2.TrueAnom = keplerianCoordinates1.TrueAnom * 10;
                }
            }
        }

        internal static void ConvertKeplerianToCartesian(KeplerianCoordinates keplerianCoordinates, CartesianCoordinates cartesianCoordinates)
        {
            try
            {
                if (keplerianCoordinates.Depth < KeplerianDepth.Exact)
                {
                    throw new ArgumentException();
                }
                keplerianCoordinates.CheckCoordinates();
            }
            catch
            {
                throw new InvalidOperationException();
            }

            //FIXME
            cartesianCoordinates.IsValid = true;
            cartesianCoordinates.Origin = keplerianCoordinates.Origin;
            cartesianCoordinates.CoordinateFrame = keplerianCoordinates.CoordinateFrame;

            cartesianCoordinates.Position = Vector<double>.Build.Random(3);
            if (cartesianCoordinates.Depth >= CartesianDepth.Velocity)
            {
                cartesianCoordinates.Velocity = Vector<double>.Build.Random(3);
            }
        }
    }
}
