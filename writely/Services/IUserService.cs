using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public interface IUserService
    {
        Task<IdentityResult> Register(UserRegistrationDto registration);
        Task<IdentityResult> DeleteAccount(string id);
        Task<IdentityResult> DisableAccount(string id);
        Task<UserData> GetUserData(string id);
    }
}
