namespace TrackingApi.BackgroundJobs.Jobs;

public interface ITrackingBackgroundJob
{
    Task UpdateAllTrackingStatuses();
}
