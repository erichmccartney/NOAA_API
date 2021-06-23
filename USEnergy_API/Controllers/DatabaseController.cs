using Microsoft.AspNetCore.Mvc;
using NOAA_API.DataAccess;
using NOAA_API.APIHandlers;
using NOAA_API.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using GeoCoordinatePortable;

namespace NOAA_API.Controllers
{
    public class DatabaseController : Controller
    {
        public ApplicationDbContext dbContext;

        public DatabaseController(ApplicationDbContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddPark()
        {
            DB.Models.Park park = new();
            park.name = "Test";
            park.latitude = 34.26F;
            park.longitude = -87.18F;
            dbContext.Parks.Add(park);

            foreach(var station in dbContext.Stations)
            {
                // For every station within 1 degree latitude and longitude,
                // record a distance relationship
                if (Math.Abs(station.latitude - park.latitude) < 1.0F
                    && Math.Abs(station.longitude - park.longitude) < 1.0F)
                {
                    var stationCoord = new GeoCoordinate(station.latitude, station.longitude);
                    var parkCoord = new GeoCoordinate(park.latitude, park.longitude);
                    var distance = stationCoord.GetDistanceTo(parkCoord);

                    DB.Models.StationParkDistance distanceRecord = new();
                    distanceRecord.stationId = station.id;
                    distanceRecord.parkId = park.id;
                    distanceRecord.distance = distance;
                    dbContext.StationParkDistances.Add(distanceRecord);
                }
            }
            dbContext.SaveChanges();

            // FIXME this doesn't seem to be returning anything.
            var results = (from d in dbContext.StationParkDistances
                           join s in dbContext.Stations on d.stationId equals s.id
                           join p in dbContext.Parks on d.parkId equals p.id
                           where p.name == park.name && p.latitude == park.latitude && p.longitude == park.longitude
                           select new DB.Models.DistanceViewModel
                           {
                               stationName = s.name,
                               parkName = p.name,
                               distance = d.distance
                           });
            return View(results.ToList());
        }

        public IActionResult ResetStations()
        {
            // Clear the Stations table.
            dbContext.Stations.RemoveRange(dbContext.Stations);

            // Get new station data from NOAA
            NOAA_APIHandler noaaHandler = new();
            Stations stations = noaaHandler.getStations();

            // Add station data to the Stations table.
            foreach (Station station in stations.results)
            {
                DB.Models.Station dbStation = new();
                dbStation.name = station.name;
                dbStation.latitude = station.latitude;
                dbStation.longitude = station.longitude;

                dbContext.Stations.Add(dbStation);
            }
            dbContext.SaveChanges();

            return View(dbContext.Stations.ToList());
        }
    }
}
