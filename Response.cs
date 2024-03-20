using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static CTA_Tracker_Client.Helpers;

namespace CTA_Tracker_Client
{
    [XmlRoot("ctatt")]
    public record Response
    {

        [XmlElement("tmst")]
        public string? ResponseTimestamp { get; set; }

        [XmlElement("errCd")]
        public int? ErrorCode { get; set; }

        [XmlElement("errNm")]
        public string? ErrorMessage { get; set; }

        [XmlElement("eta")]
        public Estimate[]? Estimates { get; set; }

    }

    [XmlRoot("eta")]
    public record Estimate
    {
        [XmlElement("staId")]
        public ushort? StationId { get; set; }

        [XmlElement("stpId")]
        public ushort? StopId { get; set; }

        [XmlElement("staNm")]
        public string? ParentStationName { get; set; }

        [XmlElement("stpDe")]
        public string? StopName { get; set; }

        [XmlElement("rn")]
        public ushort? RunNumber { get; set; }

        [XmlElement("rt")]
        public string? _routeName { get; set; }
        public string RouteName => !String.IsNullOrEmpty(_routeName) ? FullRouteName(_routeName) : "";

        [XmlElement("destSt")]
        public ushort? ServiceRunEndStation_Experimental { get; set; }

        [XmlElement("destNm")]
        public string? Destination { get; set; }

        [XmlElement("trDr")]
        public byte? DirectionCode { get; set; }

        [XmlElement("prdt")]
        public string? EstimateTimestamp { get; set; }

        [XmlElement("arrT")]
        public string? ArriveDepartTimestamp { get; set; }

        [XmlElement("isApp")]
        public byte? IsDue { get; set; }

        [XmlElement("isSch")]
        public byte? IsSchedulePrediction { get; set; }

        [XmlElement("isFlt")]
        public byte? IsFaultDetected { get; set; }

        [XmlElement("isDly")]
        public byte? IsDelayed { get; set; }

        [XmlElement("flags")]
        public string? Flags { get; set; }


        [XmlElement("lat")]
        public string? _lat { get; set; }
        public decimal? LatitudePosition => !string.IsNullOrEmpty(_lat) ? decimal.Parse(_lat) : -1;


        [XmlElement("lon")]
        public string? _lon { get; set; }
        public decimal? LongitudePosition => !string.IsNullOrEmpty(_lon) ? decimal.Parse(_lon) : -1;


        [XmlElement("heading")]
        public string? _heading { get; set; }
        public ushort? HeadingDegrees => !string.IsNullOrEmpty(_heading) ? ushort.Parse(_heading) : (ushort)0;


    }


}
