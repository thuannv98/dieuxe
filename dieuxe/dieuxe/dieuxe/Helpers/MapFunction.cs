using dieuxe.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace dieuxe.Helpers
{
    public class MapFunction
    {
        string Mapkey;
        int countRequest;
        public int CountRequest { get { return countRequest; } }
        public List<List<double>> distance { get; set; }
        public List<List<double>> duration { get; set; }

        public MapFunction(string mapkey)
        {
            this.Mapkey = mapkey;

            countRequest = 0;
        }


        public string GetGeoCodeUrl(double lat, double lng)
        {
            return "https://maps.googleapis.com/maps/api/geocode/json?latlng=" + lat + "," + lng + "&key=" + Mapkey;
        }

        public async Task<string> GetJsonAsync(string url)
        {
            var handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string result = await client.GetStringAsync(url);
            countRequest++;
            return result;
        }

        public async Task<MapDirectionResponse> GetDirectionJsonAsync(Coordinate origin, Coordinate destination, List<Coordinate> waypoints)
        {
            string str_origin = "origin=" + origin.Latitude + "," + origin.Longitude;
            string str_destination = "destination=" + destination.Latitude + "," + destination.Longitude;

            string mode = "mode=driving";

            string parameter = str_origin + "&" + str_destination + "&" + mode;
            if (waypoints != null && waypoints.Count > 0)
            {
                parameter += "&waypoints=";
                int count = waypoints.Count;
                for (int i = 0; i < count; i++) 
                {
                    if(i == count-1)
                        parameter += waypoints[i].Latitude + "," + waypoints[i].Longitude;
                    else
                        parameter += waypoints[i].Latitude + "," + waypoints[i].Longitude+"|";
                }
            }

            string output = "json";

            string key = "key="+this.Mapkey;

            string url = "https://maps.googleapis.com/maps/api/directions/" + output + "?" + parameter + "&" + key;
            System.Console.WriteLine("url: " + url);
            string json = await GetJsonAsync(url);
            //System.Console.WriteLine("js: " + json);


            return JsonConvert.DeserializeObject<MapDirectionResponse>(json);
        }

        public async Task<double> GetDistanceAsync(Coordinate origin, Coordinate destination, List<Coordinate> waypoints = null)
        {
            MapDirectionResponse mapDirectionResponse = await GetDirectionJsonAsync(origin, destination, waypoints);
            double distance = 0;
            foreach (Leg leg in mapDirectionResponse.routes[0].legs)
            {
                distance += leg.distance.value;
            }
            return distance;
        }

        public IEnumerable<Coordinate> GetPolylinePoints(MapDirectionResponse mapDirectionResponse)
        {
            string overview_polyline = mapDirectionResponse.routes[0].overview_polyline.points;
            return Decode(overview_polyline);
        }

        public static IEnumerable<Coordinate> Decode(string polylineString)
        {
            if (string.IsNullOrEmpty(polylineString))
                throw new ArgumentNullException(nameof(polylineString));

            var polylineChars = polylineString.ToCharArray();
            var index = 0;

            var currentLat = 0;
            var currentLng = 0;

            while (index < polylineChars.Length)
            {
                // Next lat
                var sum = 0;
                var shifter = 0;
                int nextFiveBits;
                do
                {
                    nextFiveBits = polylineChars[index++] - 63;
                    sum |= (nextFiveBits & 31) << shifter;
                    shifter += 5;
                } while (nextFiveBits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // Next lng
                sum = 0;
                shifter = 0;
                do
                {
                    nextFiveBits = polylineChars[index++] - 63;
                    sum |= (nextFiveBits & 31) << shifter;
                    shifter += 5;
                } while (nextFiveBits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && nextFiveBits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                yield return new Coordinate(Convert.ToDouble(currentLat) / 1E5, Convert.ToDouble(currentLng) / 1E5);
                //{
                //    Latitude = 
                //    Longitude = 
                //};
            }
        }

        public async Task<MapDistanceMatrixResponse> GetDistanceMatrixResponse(List<Coordinate> origins, List<Coordinate> destinations)
        {
            string str_origin = "origins=";
            int count = origins.Count;
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                    str_origin += origins[i].Latitude + "," + origins[i].Longitude;
                else
                    str_origin += origins[i].Latitude + "," + origins[i].Longitude + "|";
            }

            string str_destination = "destinations=";
            count = destinations.Count;
            for (int i = 0; i < count; i++)
            {
                if (i == count - 1)
                    str_destination += destinations[i].Latitude + "," + destinations[i].Longitude;
                else
                    str_destination += destinations[i].Latitude + "," + destinations[i].Longitude + "|";
            }
            string mode = "mode=driving";

            string parameter = str_origin + "&" + str_destination + "&" + mode;

            string output = "json";

            string key = "key=" + this.Mapkey;

            string url = "https://maps.googleapis.com/maps/api/distancematrix/" + output + "?" + parameter + "&" + key;
            
            //System.Console.WriteLine("Distance Matrix Request: " + url);

            string json = await GetJsonAsync(url);

            MapDistanceMatrixResponse mapDistanceMatrixResponse = JsonConvert.DeserializeObject<MapDistanceMatrixResponse>(json);
            
            return mapDistanceMatrixResponse;
        }

        public async Task<List<List<double>>> GetDistanceMatrix(List<Coordinate> origins, List<Coordinate> destinations)
        {
            MapDistanceMatrixResponse mapDistanceMatrixResponse = await GetDistanceMatrixResponse(origins, destinations);
            int rowcount = mapDistanceMatrixResponse.rows.Count;
            List<List<double>> distancematrix = new List<List<double>>();
            for (int i = 0; i < rowcount; i++)
            {
                Row row = mapDistanceMatrixResponse.rows[i];
                distancematrix.Add(new List<double>());
                foreach (Element element in row.elements)
                {
                    distancematrix[i].Add(element.distance.value);
                }
            }
            return distancematrix;
        }


        //public async Task<string> FindCordinateAddress(LatLng position)
        //{
        //    string url = GetGeoCodeUrl(position.Latitude, position.Longitude);
        //    string json = "";
        //    string placeAddress = "";

        //    //Check for Internet connection
        //    json = await GetGeoJsonAsync(url);

        //    if (!string.IsNullOrEmpty(json))
        //    {
        //        var geoCodeData = JsonConvert.DeserializeObject<GeocodingParser>(json);
        //        if (!geoCodeData.status.Contains("ZERO"))
        //        {
        //            if (geoCodeData.results[0] != null)
        //            {
        //                placeAddress = geoCodeData.results[0].formatted_address;
        //            }
        //        }
        //    }

        //    return placeAddress;
        //}
    }
}
