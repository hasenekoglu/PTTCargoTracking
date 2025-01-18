using System.Text.Json.Serialization;

namespace TrackingApi.Models
{
    public class TrackingDetail
    {
        public int Id { get; set; } // Primary key eklendi
        public DateTime Date { get; set; }
        public string Operation { get; set; }
        public string Branch { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string TrackingNumber { get; set; } // Foreign key için
        [JsonIgnore] public TrackingItem TrackingItem { get; set; } // Navigation property
    }
}
