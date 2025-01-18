using Microsoft.AspNetCore.Mvc;
using TrackingApi.Models;
using TrackingApi.Models.DTOs;
using TrackingApi.Services.Interfaces;

namespace TrackingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrackingController : ControllerBase
{
    private readonly ITrackingService _trackingService;

    public TrackingController(ITrackingService trackingService)
    {
        _trackingService = trackingService;
    }
  
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TrackingItemDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.TrackingNumber))
            return BadRequest("Tracking number is required");

        await _trackingService.AddTrackingItems(dto.TrackingNumber);
        return Ok("Tracking item added.");
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var items = await _trackingService.GetAllTrackingItems();
        return Ok(items);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _trackingService.DeleteTrackingItems(id);
        return Ok("Tracking number deleted successfully.");
    }

  
}