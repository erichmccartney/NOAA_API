using System.ComponentModel.DataAnnotations;

namespace DB.Models
{
    public class Station
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
    }
    public class Park
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }

    }

    public class StationParkDistance
    {
        [Key]
        public int id { get; set; }
        public int stationId { get; set; }
        public int parkId { get; set; }
        public double distance { get; set; }

    }

    public class DistanceViewModel
    {
        public string stationName { get; set; }
        public string parkName { get; set; }
        public double distance { get; set; }
    }
}