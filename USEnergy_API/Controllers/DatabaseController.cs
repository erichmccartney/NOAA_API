using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NOAA_API.Models;
using System;
using System.Net.Http;
using NOAA_API.DataAccess;

namespace NOAA_API.Controllers
{
    public class DatabaseController : Controller
    {
        HttpClient httpClient;

        static string BASE_URL = "https://www.ncdc.noaa.gov/cdo-web/api/v2/";  // website where data is avalible

        static string API_KEY = "HTwlDByEqCdjreGaPbbmkMYBpGpgfZKf"; //Add your API key here inside ""

        public ApplicationDbContext dbContext;

        public DatabaseController(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ResetDB()
        {
            // Clear the Stations table.
            //dbContext.Stations.RemoveRange(dbContext.Stations);

            // Get new station data from NOAA
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("token", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string NOAA_API_PATH = BASE_URL + "/stations?"; //parks?limit=20";
            string stationdata = "";

            Stations stations = null;

            httpClient.BaseAddress = new Uri(NOAA_API_PATH);

            try
            {
                HttpResponseMessage response = httpClient.GetAsync(NOAA_API_PATH).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    stationdata = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                }

                if (!stationdata.Equals(""))
                {
                    // JsonConvert is part of the NewtonSoft.Json Nuget package
                    stations = JsonConvert.DeserializeObject<Stations>(stationdata);

                    foreach (Station station in stations.results)
                    {
                        DB.Models.Station dbStation = new DB.Models.Station();
                        dbStation.name = station.name;
                        dbStation.latitude = station.latitude;
                        dbStation.longitude = station.longitude;

                        dbContext.Stations.Add(dbStation);
                        dbContext.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                // This is a useful place to insert a breakpoint and observe the error message
                Console.WriteLine(e.Message);
            }

            return View();
        }
    }
}
