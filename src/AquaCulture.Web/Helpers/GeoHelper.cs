using GoogleMaps.LocationServices;
using GoogleMapsComponents.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.Web.Helpers
{
    public class PointXY
    {
        public double Y { get; set; }
        public double X { get; set; }
        public PointXY(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
    public class GeoHelpers
    {
        public static GoogleLocationService locationService { set; get; }
        public static (double lat, double lon) GetLocationFromAddress(string Address)
        {
            //var address = "Stavanger, Norway";
            try
            {
                if (locationService == null)
                    locationService = new GoogleLocationService(Data.AppConstants.GMapApiKey);

                var point = locationService.GetLatLongFromAddress(Address);

                var latitude = point.Latitude;
                var longitude = point.Longitude;
                return (latitude, longitude);
            }
            catch (Exception)
            {

                return (0, 0);
            }

        }
        public static bool IsPointInPolygon(LatLngLiteral p, List<LatLngLiteral> poly)
        {

            int n = poly.Count();

            poly.Add(new LatLngLiteral { Lat = poly[0].Lat, Lng = poly[0].Lng });
            LatLngLiteral[] v = poly.ToArray();

            int wn = 0;    // the winding number counter

            // loop through all edges of the polygon
            for (int i = 0; i < n; i++)
            {   // edge from V[i] to V[i+1]
                if (v[i].Lat <= p.Lat)
                {         // start y <= P.y
                    if (v[i + 1].Lat > p.Lat)      // an upward crossing
                        if (isLeft(v[i], v[i + 1], p) > 0)  // P left of edge
                            ++wn;            // have a valid up intersect
                }
                else
                {                       // start y > P.y (no test needed)
                    if (v[i + 1].Lat <= p.Lat)     // a downward crossing
                        if (isLeft(v[i], v[i + 1], p) < 0)  // P right of edge
                            --wn;            // have a valid down intersect
                }
            }
            if (wn != 0)
                return true;
            else
                return false;

        }
        private static int isLeft(LatLngLiteral P0, LatLngLiteral P1, LatLngLiteral P2)
        {
            double calc = ((P1.Lng - P0.Lng) * (P2.Lat - P0.Lat)
                    - (P2.Lng - P0.Lng) * (P1.Lat - P0.Lat));
            if (calc > 0)
                return 1;
            else if (calc < 0)
                return -1;
            else
                return 0;
        }
        public static bool PolyContainsPointXY(List<PointXY> PointXYs, PointXY p)
        {
            bool inside = false;

            // An imaginary closing segment is implied,
            // so begin testing with that.
            PointXY v1 = PointXYs[PointXYs.Count - 1];

            foreach (PointXY v0 in PointXYs)
            {
                double d1 = (p.Y - v0.Y) * (v1.X - v0.X);
                double d2 = (p.X - v0.X) * (v1.Y - v0.Y);

                if (p.Y < v1.Y)
                {
                    // V1 below ray
                    if (v0.Y <= p.Y)
                    {
                        // V0 on or above ray
                        // Perform intersection test
                        if (d1 > d2)
                        {
                            inside = !inside; // Toggle state
                        }
                    }
                }
                else if (p.Y < v0.Y)
                {
                    // V1 is on or above ray, V0 is below ray
                    // Perform intersection test
                    if (d1 < d2)
                    {
                        inside = !inside; // Toggle state
                    }
                }

                v1 = v0; //Store previous endPointXY as next startPointXY
            }

            return inside;
        }
    }
    ///-------------------------------------------------------------------------------------------------
    /// <summary>   A geo tool. </summary>
    ///
    /// <remarks>   NSMaps, 5/16/2019. </remarks>
    ///-------------------------------------------------------------------------------------------------

    public class GeoTool
    {
        /// <summary>   The degrees per radians. </summary>
        static double DEG_PER_RAD = (180.0 / Math.PI);

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Calculate speed from distance and time diff. </summary>
        ///
        /// <remarks>   NSMaps, 5/16/2019. </remarks>
        ///
        /// <param name="dist">         Distance in meters. </param>
        /// <param name="timestamp1">   time 1 in milis. </param>
        /// <param name="timestamp2">   time 2 in milis. </param>
        /// <param name="unit">         . </param>
        ///
        /// <returns>   The calculated speed. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double CalculateSpeed(double dist, long timestamp1, long timestamp2, char unit)
        {

            var time_s = (timestamp2 - timestamp1) / 1000.0;
            double speed_mps = dist / time_s;
            switch (unit)
            {
                case 'k':
                case 'K':
                    double speed_kph = (speed_mps * 3600.0) / 1000.0;
                    break;
                case 'm':
                case 'M':
                default:
                    return speed_mps;
            }
            return 0;

        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Return the distance in Km from 2 lat and lon coordinates. </summary>
        ///
        /// <remarks>   NSMaps, 5/16/2019. </remarks>
        ///
        /// <param name="lat1"> Lat of start point. </param>
        /// <param name="lon1"> Lon of start point. </param>
        /// <param name="lat2"> Lat of end point. </param>
        /// <param name="lon2"> Lon of end point. </param>
        /// <param name="unit"> K for Km and N for nautical miles. </param>
        ///
        /// <returns>   Distance in Km. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double Distance(double lat1, double lon1, double? lat2, double? lon2, char unit)
        {
            if (!lat2.HasValue || !lon2.HasValue) return 0;
            double theta = lon1 - lon2.Value;
            double dist = Math.Sin(Deg2rad(lat1)) * Math.Sin(Deg2rad(lat2.Value)) + Math.Cos(Deg2rad(lat1)) * Math.Cos(Deg2rad(lat2.Value)) * Math.Cos(Deg2rad(theta));
            dist = Math.Acos(dist);
            dist = Rad2deg(dist);
            dist = dist * 60 * 1.1515;

            if (unit == 'K' || unit == 'k')  // Kilometers
            {
                dist = dist * 1.609344;
            }
            else                            // Nautical miles
            {
                dist = dist * 0.8684;
            }

            return (dist);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a range and bearing between 2 lat and long coordinates. </summary>
        ///
        /// <remarks>   NSMaps, 5/16/2019. </remarks>
        ///
        /// <param name="lat1"> Lat of start point. </param>
        /// <param name="lon1"> Lon of start point. </param>
        /// <param name="lat2"> Lat of end point. </param>
        /// <param name="lon2"> Lon of end point. </param>
        ///
        /// <returns>   A Geolocation pair. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double GetBearing(double lat1, double lon1, double lat2, double lon2)
        {
            var dLon = lon2 - lon1;
            var y = Math.Sin(dLon) * Math.Cos(lat2);
            var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
            return DEG_PER_RAD * Math.Atan2(y, x);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts decimal degress to radians. </summary>
        ///
        /// <remarks>   NSMaps, 5/16/2019. </remarks>
        ///
        /// <param name="deg">  Value to convert. </param>
        ///
        /// <returns>   Value in Radians. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double Deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Converts decimal radians to degrees. </summary>
        ///
        /// <remarks>   NSMaps, 5/16/2019. </remarks>
        ///
        /// <param name="rad">  Value to convert. </param>
        ///
        /// <returns>   Value in Degrees. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double Rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a position for a known centre point and range and bearing. </summary>
        ///
        /// <remarks>   NSMaps, 5/16/2019. </remarks>
        ///
        /// <param name="startPoint">               Location from which to range and bearing from. </param>
        /// <param name="initialBearingRadians">    Bearing in radians. </param>
        /// <param name="distanceKilometres">       Distane in Km's. </param>
        ///
        /// <returns>   Geolocaton pair in degrees. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static GeoLocation FindPointAtDistanceFrom(GeoLocation startPoint, double initialBearingRadians, double distanceKilometres)
        {
            const double radiusEarthKilometres = 6371.01;
            var distRatio = distanceKilometres / radiusEarthKilometres;
            var distRatioSine = Math.Sin(distRatio);
            var distRatioCosine = Math.Cos(distRatio);

            var startLatRad = Deg2rad(startPoint.Latitude);
            var startLonRad = Deg2rad(startPoint.Longitude);

            var startLatCos = Math.Cos(startLatRad);
            var startLatSin = Math.Sin(startLatRad);

            var endLatRads = Math.Asin((startLatSin * distRatioCosine) + (startLatCos * distRatioSine * Math.Cos(initialBearingRadians)));

            var endLonRads = startLonRad
                + Math.Atan2(
                    Math.Sin(initialBearingRadians) * distRatioSine * startLatCos,
                    distRatioCosine - startLatSin * Math.Sin(endLatRads));

            return new GeoLocation
            {
                Latitude = Rad2deg(endLatRads),
                Longitude = Rad2deg(endLonRads)
            };
        }


        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Returns a bearing between the 2 locations (for sonar form) </summary>
        ///
        /// <remarks>   NSMaps, 5/16/2019. </remarks>
        ///
        /// <param name="a1">   . </param>
        /// <param name="a2">   . </param>
        /// <param name="b1">   . </param>
        /// <param name="b2">   . </param>
        ///
        /// <returns>   Bearing value in degrees. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static double GetBearing2(double a1, double a2, double b1, double b2)
        {
            const double TWOPI = 6.2831853071795865;
            const double RAD2DEG = 57.2957795130823209;
            //
            // if (a1 = b1 and a2 = b2) throw an error 
            //
            double theta = Math.Atan2(b1 - a1, a2 - b2);
            if (theta < 0.0)
                theta += TWOPI;
            return RAD2DEG * theta;
        }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets xy from lat lon. </summary>
        ///
        /// <remarks>   NSMaps, 5/16/2019. </remarks>
        ///
        /// <param name="Position">     The position. </param>
        /// <param name="MinCoord">     The minimum coordinate. </param>
        /// <param name="MaxCoord">     The maximum coordinate. </param>
        /// <param name="WidthMap">     The width map. </param>
        /// <param name="HeightMap">    The height map. </param>
        ///
        /// <returns>   The xy from lat lon. </returns>
        ///-------------------------------------------------------------------------------------------------

        public static Point GetXYFromLatLon(GeoLocation Position, GeoLocation MinCoord, GeoLocation MaxCoord, int WidthMap, int HeightMap)
        {
            //System.Windows.Point Center, int Heading=-1
            var newPoint = new Point();
            newPoint.Y = (int)((Position.Latitude - MinCoord.Latitude) / (MaxCoord.Latitude - MinCoord.Latitude) * HeightMap);
            newPoint.X = (int)((Position.Longitude - MinCoord.Longitude) / (MaxCoord.Longitude - MinCoord.Longitude) * WidthMap);
            //if (Heading > 0)
            {
                /*
                //rotate coordinate
                double dx = item.Coordinate.X * (1f / zoom);
                double dy = item.Coordinate.Y * (1f / zoom);
                if (Angle > 0)
                {
                    System.Windows.Vector vec = new System.Windows.Vector(dx, dy);
                    var NewPoint = VectorExtentions.Rotate(new System.Windows.Point(dx, dy), Angle, centerPoint);
                    dx = NewPoint.X;
                    dy = NewPoint.Y;
                }*/
            }
            return newPoint;
        }
    }
    public class GeoLocation
    {
        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the latitude. </summary>
        ///
        /// <value> The latitude. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Latitude { set; get; }

        ///-------------------------------------------------------------------------------------------------
        /// <summary>   Gets or sets the longitude. </summary>
        ///
        /// <value> The longitude. </value>
        ///-------------------------------------------------------------------------------------------------

        public double Longitude { set; get; }
    }
}