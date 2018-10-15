using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TransportAPISharp
{

    /// <summary>
    /// The return class for <c>TransportApiClient.PlacesNear</c>
    /// </summary>
    public class PlacesNearResponse
    {
        public DateTime request_time { get; set; }
        public string source { get; set; }
        public string acknowledgements { get; set; }
        public Place[] member { get; set; }
    }

    /// <summary>
    /// Class representing places returned by TransportApiClient
    /// </summary>
    public class Place
    {
        public string type { get; set; }
        public string name { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public float accuracy { get; set; }
        public string station_code { get; set; }
        public string tiploc_code { get; set; }
        public float distance { get; set; }

        public override string ToString()
        {
            return this.name;
        }
    }

    /// <summary>
    /// The return class for <c>TransportApiClient.LiveTrains</c>
    /// </summary>
    public class LiveTrainsResponse
    {
        //public DateTime date { get; set; }
        //public DateTime time_of_day { get; set; }
        public DateTime request_time { get; set; }
        public string station_name { get; set; }
        public string station_code { get; set; }
        [JsonProperty("departures")]
        public Dictionary<string, List<TrainDeparture>> Departures { get; set; }
    }

    /// <summary>
    /// Represents a Train Departure returned by TransportApiClient
    /// </summary>
    public class TrainDeparture
    {
        public string category { get; set; }
        public string mode { get; set; }
        public string service { get; set; }
        public string train_uid { get; set; }
        [JsonProperty("platform")]
        public string Platform { get; set; }
        [JsonProperty("operator")]
        public string TrainOperator { get; set; }
        [JsonProperty(PropertyName ="aimed_departure_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime AimedDepartureTime { get; set; }
        [JsonProperty(PropertyName = "aimed_arrival_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime AimedArrivalTime { get; set; }
        [JsonProperty(PropertyName ="aimed_pass_time",NullValueHandling=NullValueHandling.Ignore)]
        public DateTime AimedPassTime { get; set; }
        public string origin_name { get; set; }
        public string source { get; set; }
        [JsonProperty("destination_name")]
        public string DestinationName { get; set; }
        public string status { get; set; }
        [JsonProperty(PropertyName = "expected_arrival_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpectedArrivalTime { get; set; }
        [JsonProperty(PropertyName ="expected_departure_time", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ExpectedDepartureTime { get; set; }
        [JsonProperty(PropertyName = "best_arrival_estimate_mins", NullValueHandling = NullValueHandling.Ignore)]
        public float BestArrivalEstimateMins { get; set; }
        [JsonProperty(PropertyName = "best_departure_estimate_mins", NullValueHandling = NullValueHandling.Ignore)]
        public float BestDepartureEstimateMins { get; set; }

        public override string ToString()
        {
            return AimedDepartureTime.ToShortTimeString() + " - " + DestinationName;
        }

    }

    /// <summary>
    /// The return class for <c>TransportApiClient.BusStopsNear</c>
    /// </summary>
    public class BusStopsNearResponse
    {
        public float minlon { get; set; }
        public float minlat { get; set; }
        public float maxlon { get; set; }
        public float maxlat { get; set; }
        public float searchlon { get; set; }
        public float searchlat { get; set; }
        public int page { get; set; }
        public int rpp { get; set; }
        public int total { get; set; }
        public DateTime request_time { get; set; }
        public Stop[] stops { get; set; }
    }

    public class Stop
    {
        public string atcocode { get; set; }
        public string smscode { get; set; }
        public string name { get; set; }
        public string mode { get; set; }
        public string bearing { get; set; }
        public string locality { get; set; }
        public string indicator { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int distance { get; set; }
    }

    public class BusTimetableResponse
    {
        [JsonProperty("atcocode")]
        public string Atcocode { get; set; }

        [JsonProperty("smscode")]
        public string Smscode { get; set; }

        [JsonProperty("request_time")]
        public DateTime RequestTime { get; set; }

        [JsonProperty("bearing")]
        public string Bearing { get; set; }

        [JsonProperty("stop_name")]
        public string StopName { get; set; }

        [JsonProperty("departures")]
        public Dictionary<string, List<BusTimetableDeparture>> Departures { get; set; }
    }

    public class BusTimetableDeparture
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("aimed_departure_time")]
        public TimeSpan AimedDepartureTime { get; set; }

        [JsonProperty("dir")]
        public string Dir { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonIgnore]
        public DateTime AimedDepartureDateTime
        {
            get
            {
                return new DateTime(Date.Year, Date.Month, Date.Day, AimedDepartureTime.Hours, AimedDepartureTime.Minutes, AimedDepartureTime.Seconds);
            }
        }
    }

    public class BusLiveDeparture
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("aimed_departure_time")]
        public TimeSpan? AimedDepartureTime { get; set; }

        [JsonProperty("expected_departure_time")]
        public TimeSpan? ExpectedDepartureTime { get; set; }

        [JsonProperty("best_departure_estimate")]
        public TimeSpan BestDepartureEstimate { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonIgnore]
        public DateTime BestDepartureEstimateDateTime
        {
            get
            {
                var dtNow = DateTime.Now;

                var timeNow = dtNow.TimeOfDay;

                // If the BestDepartureEstimate is less than the time now, it's tomorrow, so add a
                // day to dtNow...
                if (BestDepartureEstimate < timeNow)
                    dtNow.AddDays(1);

                return new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, BestDepartureEstimate.Hours, BestDepartureEstimate.Minutes, 0);
            }
        }
    }

    public class BusLiveResponse
    {
        [JsonProperty("atcocode")]
        public string Atcocode { get; set; }

        [JsonProperty("smscode")]
        public string Smscode { get; set; }

        [JsonProperty("request_time")]
        public DateTime RequestTime { get; set; }

        [JsonProperty("departures")]
        public Dictionary<string, List<BusLiveDeparture>> Departures { get; set; }
    }

    public class BusOperator
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("refid")]
        public string Refid { get; set; }
    }

    public class BusRouteTimetableStop
    {
        [JsonProperty("time")]
        public TimeSpan Time { get; set; }

        [JsonProperty("atcocode")]
        public string Atcocode { get; set; }

        [JsonProperty("smscode")]
        public string Smscode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }

        [JsonProperty("indicator")]
        public string Indicator { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("bearing")]
        public string Bearing { get; set; }
    }

    public class BusRouteTimetableResponse
    {
        [JsonProperty("request_time")]
        public DateTime RequestTime { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }

        [JsonProperty("origin_atcocode")]
        public string OriginAtcocode { get; set; }

        [JsonProperty("stops")]
        public IList<BusRouteTimetableStop> Stops { get; set; }
    }
}