using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using writely.Models;
using writely.Models.Dto;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace writely.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<AppUser> _manager;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(SignInManager<AppUser> manager, UserManager<AppUser> userManager)
        {
            _manager = manager;
            _userManager = userManager;
        }

        public async Task<SignInResult> Login(UserLoginDto creds)
        {
            var user = await _userManager.FindByEmailAsync(creds.Email);
            if (user == null)
            {
                return SignInResult.Failed;
            }
            
            return await _manager.PasswordSignInAsync(user.UserName, creds.Password, creds.RememberMe, true);
        }

        public async Task Logout() => await _manager.SignOutAsync();
    }
}