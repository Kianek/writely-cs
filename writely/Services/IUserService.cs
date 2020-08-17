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
        Task<IdentityResult> ActivateAccount(string id);
        Task<IdentityResult> ChangePassword(string id, string oldPassword, string newPassword);
        Task<IdentityResult> DisableAccount(string id);
        Task<UserData> GetUserData(IUserDataService dataService, string id);
    }
}
