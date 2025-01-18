using System.ComponentModel.DataAnnotations;

namespace TrackingApi.Models;

public class TrackingItem
{
    [Key] public int Id { get; set; }
    public string TrackingNumber { get; set; }
    public string Status { get; set; }
    public DateTime LastUpdate { get; set; }
    public List<TrackingDetail> Details { get; set; }

}

