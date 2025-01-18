using TrackingApi.Models;

namespace TrackingApi.Services.Interfaces;

    public interface ITrackingService
    {
        Task AddTrackingItems(string number);
        Task<IEnumerable<TrackingItem>> GetAllTrackingItems();
        Task DeleteTrackingItems(int id);
    }

