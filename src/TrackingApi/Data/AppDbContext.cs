using Microsoft.EntityFrameworkCore;
using TrackingApi.Models;

namespace TrackingApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TrackingItem> TrackingItems { get; set; }
    public DbSet<TrackingDetail> TrackingDetails { get; set; }

}

