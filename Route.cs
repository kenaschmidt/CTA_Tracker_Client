using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CTA_Tracker_Client
{

    public class RouteColorShortCodeAttribute : Attribute
    {
        public string Code { get; init; }

        public RouteColorShortCodeAttribute(string code)
        {
            Code = code;
        }
    }


    /// <summary>
    /// Named CTA 'L' train routes
    /// </summary>
    public enum RouteColor
    {
        [RouteColorShortCode("RED")]
        Red = 0,
        [RouteColorShortCode("G")]
        Green = 1,
        [RouteColorShortCode("BLUE")]
        Blue = 2,
        [RouteColorShortCode("Pnk")]
        Pink = 3,
        [RouteColorShortCode("P")]
        Purple = 4,
        [RouteColorShortCode("Pexp")]
        PurpleExp = 5,
        [RouteColorShortCode("BRN")]
        Brown = 6,
        [RouteColorShortCode("Y")]
        Yellow = 7,
        [RouteColorShortCode("O")]
        Orange = 8
    }

    /// <summary>
    /// Represents a color-designated CTA 'L' train route.  Contains a collection of Stations.
    /// </summary>
    public record Route    {


        private static List<Route>? _allRoutes { get; set; }
        public static List<Route> AllRoutes
        {
            get
            {
                if (_allRoutes == null)
                {
                    _initAllRoutes();
                }
                return _allRoutes ?? throw new NullReferenceException();
            }
        }

        private static void _initAllRoutes()
        {
            _allRoutes = new List<Route>();

            foreach (RouteColor color in Enum.GetValues(typeof(RouteColor)))
            {
                var newRoute = new Route(color);
                newRoute.Stations = new List<Station>();
                var pName = color.GetAttributeOfType<RouteColorShortCodeAttribute>();

                newRoute.Stations.AddRange(Station.AllStations.Where(sta =>
                sta.Stops.Any(stp =>
                    (bool)stp.GetType().GetProperty(((RouteColorShortCodeAttribute)pName).Code).GetValue(stp) == true)
                ));

                _allRoutes?.Add(newRoute);
            }
        }

        public static Route ByColor(RouteColor color)
        {
            return AllRoutes.Where(rt => rt.Color == color).Single();
        }

        public RouteColor Color { get; init; }
        public List<Station>? Stations { get; private set; }

        private Route(RouteColor color)
        {
            this.Color = color;
        }

    }
}
