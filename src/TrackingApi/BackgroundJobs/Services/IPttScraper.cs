using TrackingApi.Models;

namespace TrackingApi.BackgroundJobs.Services
{
    public interface IPttScraper
    {
        public Task<TrackingItem> GetTrackingStatus(string trackingNumber);
    }
}
