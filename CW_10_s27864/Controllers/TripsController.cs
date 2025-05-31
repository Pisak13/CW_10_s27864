using CW_10_s27864.DTO_s;
using CW_10_s27864.Exceptions;
using CW_10_s27864.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_10_s27864.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TripsController:ControllerBase
{
    private readonly IDbService _dbService;

    public TripsController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTripsAsync([FromQuery]int page = 1, [FromQuery]int pageSize = 10)
    {
        var trips = await _dbService.GetTripsAsync(page,pageSize);
        return Ok(trips);
    }
    
    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip([FromRoute]int idTrip,[FromBody]AddClientToTripDto dto)
    {
        dto.IdTrip = idTrip;
        try
        {
            await _dbService.AddClientToTripAsync(dto);
            return Ok("Client successfully added to trip");
        }
        catch (NotFoundException e)
        {
            return NotFound(e);
        }
    }
}