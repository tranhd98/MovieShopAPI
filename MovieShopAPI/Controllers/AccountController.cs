using ApplicationCore.Contracts.Services;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private IAccountService _accountService;
    
    // GET
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    
    [Route("register")]
    [HttpPost]
    public async Task<IActionResult> Register(UserRegisterRequestModel model)
    {
        // 2XX 
        if (!ModelState.IsValid)
        {
            return BadRequest(model);
        }
        var user = await _accountService.CreateUser(model);
        if (user == null) return BadRequest();
        return Ok(user);
    }
    
}