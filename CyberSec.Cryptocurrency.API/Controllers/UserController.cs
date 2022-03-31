using CyberSec.Cryptocurrency.API.Attributes;
using CyberSec.Cryptocurrency.API.Controllers;
using CyberSec.Cryptocurrency.Service.Enums;
using CyberSec.Cryptocurrency.Service.Interfaces;
using CyberSec.Cryptocurrency.Service.Models;
using Microsoft.AspNetCore.Mvc;

namespace CyberSec.Cryptocurrency.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticateRequest request)
        {
            var response = await _userService.AuthenticateAsync(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request)
        {
            await _userService.RegisterAsync(request, Request.Headers["origin"]);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(VerifyEmailRequest request)
        {
            await _userService.VerifyEmailAsync(request.Token);
            return Ok();
        }

        [Authorize(Role.Admin)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userService.GetAsync(id);
            return Ok(user);
        }
    }
}
