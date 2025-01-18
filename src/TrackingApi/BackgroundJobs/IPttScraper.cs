using TrackingApi.Models;

namespace TrackingApi.BackgroundJobs
{
    public interface IPttScraper
    {
        public Task<TrackingItem> GetTrackingStatus(string trackingNumber);
    }
}
