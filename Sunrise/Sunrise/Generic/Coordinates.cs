using System;
using System.Collections.Generic;
using System.Text;

using Sunrise.Generic.Frames;
using Sunrise.CelestialObjects;
using System.Linq;

namespace Sunrise.Generic
{
    public enum CoordinateType
    {
        Keplerian,
        Cartesian,
        BodyCentric,
        TopoCentric,
    }

    public abstract class BasicCoordinates
    {
        public Frame Frame { get; set; }
        public Origin Origin { get; set; }
    }

    public class KeplerianCoordinates : BasicCoordinates
    {
        public double SMA { get; set; }
        public double Ecc { get; set; }
        public double Inc { get; set; }
        public double RAAN { get; set; }
        public double ArgPer { get; set; }
        public double TA { get; set; }

        public KeplerianCoordinates()
        {
            Frame = Frame.EME2000;
            Origin = Origin.Sun;
            SMA = 0D;
            Ecc = 0D;
            Inc = 0D;
            RAAN = 0D;
            ArgPer = 0D;
            TA = 0D;
        }

        public KeplerianCoordinates(double sma, double ecc, double inc, double raan, double argPer, double ta, Frame frame, Origin origin)
        {
            SMA = sma;
            Ecc = ecc;
            Inc = inc;
            RAAN = raan;
            ArgPer = argPer;
            TA = ta;
            Frame = frame;
            Origin = origin;
        }

        internal void CheckValidity()
        {
            throw new NotImplementedException();
        }
    }

    public class Position
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        public Position()
        {
            X = 0D;
            Y = 0D;
            Z = 0D;
        }

        public Position(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class Velocity
    {
        double X { get; set; }
        double Y { get; set; }
        double Z { get; set; }

        public Velocity()
        {
            X = 0D;
            Y = 0D;
            Z = 0D;
        }

        public Velocity(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public class CartesianCoordinates : BasicCoordinates
    {
        public Position Position { get; set; }
        public Velocity Velocity { get; set; }
        public CartesianCoordinates()
        {
            Frame = Frame.EME2000;
            Origin = Origin.Sun;
            Position = new Position();
            Velocity = new Velocity();
        }

        public CartesianCoordinates(Position position, Velocity velocity, Frame frame, Origin origin)
        {
            Frame = frame;
            Origin = origin;
            Position = position;
            Velocity = velocity;
        }

        internal void CheckValidity()
        {
            throw new NotImplementedException();
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

    public abstract class BodyCoordinates
    {
        public CelestialBody Body { get; set; }
        public Frame BodyFrame { get; set; }
    }

    public class BodyCentricCoordinates : BodyCoordinates
    {
        public LatLon LatLon { get; set; }
        public double Altitude { get; set; }
        public BodyCentricCoordinates()
        {
            LatLon = new LatLon();
            Altitude = 0D;
            Body = CelestialBodies.Earth;
            BodyFrame = CelestialBodies.Earth.Frames.First();
        }

        public BodyCentricCoordinates(LatLon latLon, double altitude, CelestialBody body, Frame bodyFrame)
        {
            Body = body;
            BodyFrame = bodyFrame;
            LatLon = latLon;
            Altitude = altitude;
        }

        internal void CheckValidity()
        {
            throw new NotImplementedException();
        }
    }

    public abstract class LocalCoordinates
    {
        public Frame LocalFrame { get; set; }
    }

    public class TopoCentricCoordinates : LocalCoordinates
    {
        public BodyCentricCoordinates Location { get; set; }
        public double Azimuth { get; set; }
        public double Elevation { get; set; }
        public double Range { get; set; }

        public TopoCentricCoordinates()
        {
            LocalFrame = Frame.ENU;
            Location = new BodyCentricCoordinates();
            Azimuth = 0D;
            Elevation = 0D;
            Range = 0D;
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

    public class Coordinates
    {
        public CoordinateType? Type { get; set; }
        public KeplerianCoordinates KeplerianCoordinates { get; set; }
        public CartesianCoordinates CartesianCoordinates { get; set; }
        public BodyCentricCoordinates BodyCentricCoordinates { get; set; }
        public TopoCentricCoordinates TopoCentricCoordinates { get; set; }

        public Coordinates()
        {
            Type = CoordinateType.Cartesian;
            CartesianCoordinates = new CartesianCoordinates();
        }

        public Coordinates Convert(CoordinateType to)
        {
            if (to == CoordinateType.Cartesian && CartesianCoordinates != null)
            {
                KeplerianToCartesian(this);
            }
            else if(to == CoordinateType.Keplerian && KeplerianCoordinates != null)
            {
                CartesianToKeplerian(this);
            }
            return this;
        }

        private void GeocentricToCartesian(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        private void CartesianToGeocentric(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        private void CartesianToKeplerian(Coordinates coordinates)
        {
            coordinates.KeplerianCoordinates = new KeplerianCoordinates();
        }

        private void KeplerianToCartesian(Coordinates coordinates)
        {
            coordinates.CartesianCoordinates = new CartesianCoordinates();
        }

        public void CheckValidity()
        {
            string errorMessage = "Coordinates.CheckValidity():";
            if (Type == CoordinateType.Keplerian)
            {
                try
                {
                    KeplerianCoordinates.CheckValidity();
                }
                catch
                {
                    errorMessage += "Invalid Keplerian coordinates";
                    throw new ArgumentException(errorMessage);
                }
            }
            else if (Type == CoordinateType.Cartesian)
            {
                try
                {
                    CartesianCoordinates.CheckValidity();
                }
                catch
                {
                    errorMessage += "Invalid Cartesian coordinates";
                    throw new ArgumentException(errorMessage);
                }
            }
            else if (Type == CoordinateType.BodyCentric)
            {
                try
                {
                    BodyCentricCoordinates.CheckValidity();
                }
                catch
                {
                    errorMessage += "Invalid BodyCentric coordinates";
                    throw new ArgumentException(errorMessage);
                }
            }
            else if (Type == CoordinateType.TopoCentric)
            {
                try
                {
                    TopoCentricCoordinates.CheckValidity();
                }
                catch
                {
                    errorMessage += "Invalid TopoCentric coordinates";
                    throw new ArgumentException(errorMessage);
                }
            }
        }
    }

}
