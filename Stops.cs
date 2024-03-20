using FileHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CTA_Tracker_Client
{

    /// <summary>
    /// Represents an individual CTA 'L' Stop/Platform at a Station.  May be associated with a single station and multiple routes.
    /// </summary>
    public record Stop
    {

        private static List<Stop>? _allStops { get; set; }
        public static List<Stop> AllStops
        {
            get
            {
                if (_allStops == null)
                {
                    _initAllStops();
                }
                return _allStops ?? throw new NullReferenceException();
            }
        }

        /// <summary>
        /// Parses CSV Resource to build a static list of all stops as provided at:
        /// https://data.cityofchicago.org/Transportation/CTA-System-Information-List-of-L-Stops/8pix-ypme/data_preview
        /// </summary>
        private static void _initAllStops()
        {

            _allStops = new List<Stop>();

            // Parse CSV resource to build a list of all valid CTA stops
            var csvText = Properties.Resources.ListOfLStopsCSV.Split("\n").ToList();
            var headers = csvText[0].Split(',').ToList();

            csvText.RemoveAt(0);
            csvText.RemoveAll(x => x == string.Empty);

            foreach (var line in csvText)
            {

                var split = line.Split(',');

                // Since the CTA included commas in text strings within the CSV file, we need to manually recombine split values
                // that have opening and closing double quotes.  Clumsy to do it this way but REGEX was a pain in the butt and this works fine.

                for (int i = 0; i < split.Length; i++)
                {
                    bool re = false;
                    if (split[i].Contains('\"'))
                    {
                        re = true;
                        int c = 1;

                        while (re)
                        {
                            if (split[i + c].Contains('\"'))
                                re = false;
                            split[i] = split[i] + split[i + c];
                            split[i + c] = string.Empty;
                            c += 1;
                        }
                    }
                }

                var entry = split.Where(x => x != string.Empty).ToList();

                var stop = new Stop()
                {
                    STOP_ID = ushort.Parse(entry[headers.IndexOf("STOP_ID")]),
                    DIRECTION_ID = Char.Parse(entry[headers.IndexOf("DIRECTION_ID")]),
                    STOP_NAME = entry[headers.IndexOf("STOP_NAME")],
                    STATION_NAME = entry[headers.IndexOf("STATION_NAME")],
                    STATION_DESCRIPTIVE_NAME = entry[headers.IndexOf("STATION_DESCRIPTIVE_NAME")],
                    MAP_ID = ushort.Parse(entry[headers.IndexOf("MAP_ID")]),
                    ADA = bool.Parse(entry[headers.IndexOf("ADA")]),
                    RED = bool.Parse(entry[headers.IndexOf("RED")]),
                    BLUE = bool.Parse(entry[headers.IndexOf("BLUE")]),
                    G = bool.Parse(entry[headers.IndexOf("G")]),
                    BRN = bool.Parse(entry[headers.IndexOf("BRN")]),
                    P = bool.Parse(entry[headers.IndexOf("P")]),
                    Pexp = bool.Parse(entry[headers.IndexOf("Pexp")]),
                    Y = bool.Parse(entry[headers.IndexOf("Y")]),
                    Pnk = bool.Parse(entry[headers.IndexOf("Pnk")]),
                    O = bool.Parse(entry[headers.IndexOf("O")]),
                    Location = entry[headers.IndexOf("Location")]
                };
                _allStops.Add(stop);
            }
        }

        public ushort STOP_ID { get; init; }
        public char DIRECTION_ID { get; init; }
        public string? STOP_NAME { get; init; }
        public string? STATION_NAME { get; init; }
        public string? STATION_DESCRIPTIVE_NAME { get; init; }
        public ushort MAP_ID { get; init; }
        public bool ADA { get; init; }
        public bool RED { get; init; }
        public bool BLUE { get; init; }
        public bool G { get; init; }
        public bool BRN { get; init; }
        public bool P { get; init; }
        public bool Pexp { get; init; }
        public bool Y { get; init; }
        public bool Pnk { get; init; }
        public bool O { get; init; }
        public string? Location { get; init; }

        public List<RouteColor> StopRouteColors
        {
            get
            {
                var ret = new List<RouteColor>();
                if (RED) ret.Add(RouteColor.Red);
                if (BLUE) ret.Add(RouteColor.Blue);
                if (G) ret.Add(RouteColor.Green);
                if (BRN) ret.Add(RouteColor.Brown);
                if (P) ret.Add(RouteColor.Purple);
                if (Pexp) ret.Add(RouteColor.PurpleExp);
                if (Y) ret.Add(RouteColor.Yellow);
                if (Pnk) ret.Add(RouteColor.Pink);
                if (O) ret.Add(RouteColor.Orange);

                return ret;
            }
        }
    }
}
