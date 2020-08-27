using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using writely.Models.Dto;
using writely.Services;

namespace writely.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController
    {
        private ILogger<UsersController> _logger;
        private IUserService _userService;
        
        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _logger = logger;
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Register(UserRegistrationDto registration)
        {
            var result = await _userService.Register(registration);
            if (result.Succeeded)
            {
                _logger.LogInformation("User registered");
                return new OkObjectResult("Registration successful");
            }
            
            _logger.LogError("Unable to register user");
            return new BadRequestObjectResult(result.Errors.ToList());
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(string id)
        {
            var result = await _userService.DeleteAccount(id);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Account deleted for user: {id}");
                return new OkResult();
            }

            _logger.LogError($"Unable to delete account for user: {id}");
            return new BadRequestResult();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisableAccount(string id)
        {
            var result = await _userService.DisableAccount(id);
            if (result.Succeeded)
            {
                _logger.LogInformation($"Account disabled for user: {id}");
                return new OkResult();
            }
            
            _logger.LogError($"Unable to disable account for user: {id}");
            return new BadRequestObjectResult("Unable to disable account");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ChangePassword(string id, [FromBody]UpdatePasswordDto info)
        {
            var result = await _userService.ChangePassword(id, info.OldPassword, info.NewPassword);
            if (result.Succeeded)
            {
                return new OkResult();
            }
            
            return new BadRequestResult();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetUserData(
            string id,
            [FromServices] IUserDataService dataService)
        {
            var userData = await _userService.GetUserData(dataService, id);
            if (userData == null)
            {
                return new NotFoundObjectResult(id);
            }

            return new OkObjectResult(userData);
        }
    }
}