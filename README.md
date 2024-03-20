![image](https://github.com/kenaschmidt/CTA_Tracker_Client/assets/59780790/61c03600-e467-4549-89f5-3ffdc9a9d59c)

# CTA_Tracker_Client

Client to interface with the Chicago Transit Authority public API for tracking buses and trains.  

Initial implementation of 'L' train tracking only.

Requires free API key: https://www.transitchicago.com/developers/

Train Tracker API: https://www.transitchicago.com/developers/traintracker/

## Interface Model

**`Station`** represents a single 'L' station which has one or more `Stop`.  API refers to this by `MAP_ID` or `staId` or `mapid`

**`Stop`** is associated with a `Station` and represents a platform or direction within a `Station`.  API refers to this by `STOP_ID` or `stpid` or `stpId`

**`Route`** is a collection of `Stop` and is associated with a named `RouteColor` (Red, Pink, Brown, etc.)

**`Response`** object is the parsed response from the CTA (XML).  Contains a timestamp, error code/message, and a collection of `Estimate` which has details on each expected arrival.

Stations, Stops, and Routes are all included as static resources which are loaded from the incuded CSV file on access. (available at https://data.cityofchicago.org/Transportation/CTA-System-Information-List-of-L-Stops/8pix-ypme/data_preview).  Access these collections through:

`Station.AllStations`

`Route.AllRoutes`

`Stop.AllStops`

## Usage

-Initialize a client with your API key:

  `var client = new Client({ApiKey});`

Currently supports requesting updates for either an individual `Station` or `Stop`

### Get a station code

-Select from the list of all stations
`var station = Station.AllStations.First();`

-Select from all stations in a route
`var station = Route.ByColor(RouteColor.Red).Stations.First();`

-Select from the list of all stops
`var station = Stop.AllStops.First().MAP_ID`

### Request updates for a single station

`var response = await client.RequestUpdateByStationID(station.MapID);`

### Request updates for a single stop

`var response = await client.RequestUpdateByStopID(stop.STOP_ID);`

## Notes

-API is limited to 50,000 calls per day.  Client has no pacing or timing functions yet.
