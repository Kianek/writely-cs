using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using writely.Models.Dto;
using writely.Services;

namespace writely.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuthService _service;

        public AuthController(ILogger<AuthController> logger, IAuthService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpPost("login")]
        public async Task<ActionResult> SignIn(UserLoginDto credentials)
        {
            var result = await _service.Login(credentials);
            if (result.Succeeded)
            {
                _logger.LogInformation($"User {credentials.Email} logged in successfully");
                return Ok();
            }
            
            _logger.LogInformation($"Log in unsuccessful");
            return BadRequest();
        }

        [HttpGet("logout")]
        public async Task<ActionResult> SignOut()
        {
            throw new NotImplementedException();
        }
    }
}