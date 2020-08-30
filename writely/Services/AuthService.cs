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

        public AuthService(SignInManager<AppUser> manager)
        {
            _manager = manager;
        }

        public Task<SignInResult> Login(UserLoginDto credentials)
        {
            throw new System.NotImplementedException();
        }

        public Task Logout()
        {
            throw new System.NotImplementedException();
        }
    }
}