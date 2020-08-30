using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using writely.Models.Dto;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace writely.Services
{
    public interface IAuthService
    {
        Task<SignInResult> Login(UserLoginDto credentials);
        Task Logout();
    }
}