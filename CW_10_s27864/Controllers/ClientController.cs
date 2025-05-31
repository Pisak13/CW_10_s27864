using CW_10_s27864.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW_10_s27864.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ClientController:ControllerBase
{
    private readonly IDbService _dbService;
    
    public ClientController(IDbService dbService)
    {
        _dbService = dbService;
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveClientAsync([FromRoute]int id)
    {
        try
        {
            await _dbService.RemoveClientAsync(id);
            return NoContent();
        }catch(Exception e)
        {
            return NotFound(e);
        }
    }
}