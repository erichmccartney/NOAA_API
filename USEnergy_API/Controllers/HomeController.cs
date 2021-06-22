using Form.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NOAA_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NOAA_API.Controllers
{
    public class HomeController : Controller
    {
        HttpClient httpClient;

        static string BASE_URL = "https://www.ncdc.noaa.gov/cdo-web/api/v2/";  // website where data is avalible

        static string API_KEY = "HTwlDByEqCdjreGaPbbmkMYBpGpgfZKf"; //Add your API key here inside ""

        // https://www.ncdc.noaa.gov/cdo-web/webservices/v2#gettingStarted
        // https://www.ncdc.noaa.gov/cdo-web/token

        public IActionResult Index()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Add("token", API_KEY);
            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            string NOAA_API_PATH = BASE_URL + "/stations?"; //parks?limit=20";
            string stationdata = "";

            Stations stations = null;

            httpClient.BaseAddress = new Uri(NOAA_API_PATH);

            //try
            //{
            //    HttpResponseMessage response = httpClient.GetAsync(NOAA_API_PATH).GetAwaiter().GetResult();

            //    if (response.IsSuccessStatusCode)
            //    {
            //        stationdata = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            //    }

            //    if (!stationdata.Equals(""))
            //    {
            //        // JsonConvert is part of the NewtonSoft.Json Nuget package
            //        stations = JsonConvert.DeserializeObject<Stations>(stationdata);
            //    }
            //}
            //catch (Exception e)
            //{
            //    // This is a useful place to insert a breakpoint and observe the error message
            //    Console.WriteLine(e.Message);
            //}
            stations = new Stations();
            Station station = new Station();
            station.id = "test";
            station.latitude = 0.0F;
            station.longitude = 0.0F;
            station.name = "test";
            station.mindate = "test";
            station.maxdate = "test";
            station.datacoverage = 0.0F;
            station.elevation = 0.0F;
            station.elevationUnit = "test";
            stations.results = new Station[] { station };
            return View(stations);

        }
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            ContactForm contactform = new ContactForm() { Name = 2, Email = 2 , Result = 4};

            return View(contactform);
        }

        [HttpPost]
        public IActionResult ContactUs(ContactForm contactform)
        {
            contactform.Result = contactform.Name + contactform.Email;
            return View(contactform);
        }
    }
}