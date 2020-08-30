using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using writely.Models.Dto;
using writely.Services;

namespace writely.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<ActionResult> SignIn(UserLoginDto credentials)
        {
            throw new NotImplementedException();
        }

        [HttpGet("logout")]
        public async Task<ActionResult> SignOut()
        {
            throw new NotImplementedException();
        }
    }
}