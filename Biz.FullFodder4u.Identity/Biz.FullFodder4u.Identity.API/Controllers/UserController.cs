using Biz.FullFodder4u.Identity.API.DTOs;
using Biz.FullFodder4u.Identity.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Biz.FullFodder4u.Identity.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromForm] SignUpDataDto payload)
    {
        await _userService.SignUp(payload);
        return NoContent();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromForm] SignInDataDto payload)
    {
        var token = await _userService.SignIn(payload);
        if (token == null)
        {
            return BadRequest();
        }
        return Ok(token);
    }
}
