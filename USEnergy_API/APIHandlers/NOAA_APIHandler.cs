using System.Net.Http;
using Newtonsoft.Json;
using NOAA_API.Models;
using System;

namespace NOAA_API.APIHandlers
{
    public class NOAA_APIHandler
    {
        HttpClient httpClient;
        static string NOAA_API_PATH = "https://www.ncdc.noaa.gov/cdo-web/api/v2/stations?";
        static string API_KEY = "HTwlDByEqCdjreGaPbbmkMYBpGpgfZKf";

        public NOAA_APIHandler()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("token", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.BaseAddress = new Uri(NOAA_API_PATH);
        }

        public Stations getStations()
        {
            Stations stations = null;
            try
            {
                HttpResponseMessage response = httpClient.GetAsync(NOAA_API_PATH).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string stationdata = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    if (!stationdata.Equals(""))
                    {
                        // JsonConvert is part of the NewtonSoft.Json Nuget package
                        stations = JsonConvert.DeserializeObject<Stations>(stationdata);
                    }
                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }
            return stations;
        }
    }
}
