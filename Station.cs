using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace CTA_Tracker_Client
{
    /// <summary>
    /// Represents a CTA 'L' Station (Parent) which contains a collection of Stops/Platforms.  May be associated with multiple routes
    /// </summary>
    public class Station
    {
        // Constructor fields
        private static List<Station>? _allStations { get; set; }
        public static List<Station> AllStations
        {
            get
            {
                if (_allStations == null)
                {
                    _initAllStations();
                }
                return _allStations ?? throw new NullReferenceException();
            }
        }

        private static void _initAllStations()
        {
            _allStations = new List<Station>();
            var stations = CTA_Tracker_Client.Stop.AllStops.Select(s => (s.MAP_ID, s.STATION_NAME, s.STATION_DESCRIPTIVE_NAME)).Distinct().ToList();

            foreach (var s in stations)
            {
                var newStation = new Station(
                    s.MAP_ID,
                    s.STATION_NAME ?? throw new NullReferenceException(),
                    s.STATION_DESCRIPTIVE_NAME ?? throw new NullReferenceException());

                newStation.Stops = [.. CTA_Tracker_Client.Stop.AllStops.Where(x => x.MAP_ID == s.MAP_ID)];

                _allStations.Add(newStation);
            }
        }

        // Object fields
        public ushort MapID { get; init; }
        public string StationName { get; init; }
        public string StationDescriptiveName { get; init; }

        public List<Stop>? Stops { get; private set; }
        public List<Route>? Routes
        {
            get
            {
                var ret = new List<Route>();
                var routeColors = Stops.SelectMany(x => x.StopRouteColors).Distinct().ToList();
                foreach (var routeColor in routeColors)
                {
                    ret.Add(Route.ByColor(routeColor));
                }
                return ret;
            }
        }

        private Station(ushort mapID, string stationName, string stationDescriptiveName)
        {
            MapID = mapID;
            StationName = stationName;
            StationDescriptiveName = stationDescriptiveName;
        }
    }

}
