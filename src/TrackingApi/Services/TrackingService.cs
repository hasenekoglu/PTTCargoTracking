
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TrackingApi.Data;
using TrackingApi.Models;
using TrackingApi.Models.DTOs;
using TrackingApi.Services.Interfaces;
using TrackingApi.BackgroundJobs.Services;

namespace TrackingApi.Services;

public class TrackingService : ITrackingService
{
    private readonly AppDbContext _context;
    private readonly IPttScraper _pttScraper;
    private readonly ILogger<TrackingService> _logger;


    public TrackingService(AppDbContext context, IPttScraper pttScraper, ILogger<TrackingService> logger)
    {
        _context = context;
        _pttScraper = pttScraper;
        _logger = logger;
    }

    public async Task AddTrackingItems(string number)
    {
        var existingTrackingItem = await _context.TrackingItems
            .FirstOrDefaultAsync(t => t.TrackingNumber == number);

        if (existingTrackingItem != null)
            throw new InvalidOperationException($"Tracking number {number} already exists.");

        var trackingItem = new TrackingItem
        {
            TrackingNumber = number,
            Status = "Pending", // Durum varsayılan olarak "Pending"
            LastUpdate = DateTime.Now // Güncel tarih
        };

        _context.TrackingItems.Add(trackingItem);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TrackingItem>> GetAllTrackingItems()
    {
        var trackingItems = await _context.TrackingItems.ToListAsync();
        foreach (var item in trackingItems)
        {
            try
            {
                var trackingResult = await _pttScraper.GetTrackingStatus(item.TrackingNumber);
                item.Status = trackingResult.Status;
                item.LastUpdate = trackingResult.LastUpdate;

                if (item.Details != null)
                    _context.TrackingDetails.RemoveRange(item.Details);
                

                item.Details = trackingResult.Details.Select(d => new TrackingDetail
                {
                    Date = d.Date,
                    Operation = d.Operation,
                    Branch = d.Branch,
                    Location = d.Location,
                    Description = d.Description,
                    TrackingNumber = item.TrackingNumber
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating tracking number {TrackingNumber}", item.TrackingNumber);
            }
        }
    
        await _context.SaveChangesAsync();
        return trackingItems;
    }

    public async Task DeleteTrackingItems(int id)
    {
        var item = await _context.TrackingItems.FindAsync(id);
        if (item != null)
        {
            _context.TrackingItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

}

