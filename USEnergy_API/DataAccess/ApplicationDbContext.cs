using Microsoft.EntityFrameworkCore;
using DB.Models;

namespace NOAA_API.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Station> Stations { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<StationParkDistance> StationParkDistances { get; set; }

    }
}