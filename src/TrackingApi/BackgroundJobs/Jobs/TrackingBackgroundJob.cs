using TrackingApi.Services.Interfaces;

namespace TrackingApi.BackgroundJobs.Jobs;

public class TrackingBackgroundJob : ITrackingBackgroundJob
{
    private readonly ITrackingService _trackingService;
    private readonly ILogger<TrackingBackgroundJob> _logger;

    public TrackingBackgroundJob(ITrackingService trackingService, ILogger<TrackingBackgroundJob> logger)
    {
        _trackingService = trackingService;
        _logger = logger;
    }

    public async Task UpdateAllTrackingStatuses()
    {
        try
        {
            _logger.LogInformation("Starting background tracking update job");
            await _trackingService.GetAllTrackingItems();
            _logger.LogInformation("Completed background tracking update job");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating tracking statuses");
            throw;
        }
    }
}