using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CastController: ControllerBase
{
    private readonly ICastService _castService;

    public CastController(ICastService castService)
    {
        _castService = castService;
    }

    [Route("{id=int}")]
    [HttpGet]
    public async Task<IActionResult> GetCastInfo(int id)
    {
        var cast = await _castService.GetCastDetails(id);
        if (cast == null)
        {
            return NotFound(new {error = "Not found"});
        }

        return Ok(cast);
    }
    
}