using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public class UserService : IUserService
    {
        private UserManager<AppUser> userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        public Task<IdentityResult> Register(UserRegistrationDto registration)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAccount(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DisableAccount(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UserData> GetUserData(string id)
        {
            throw new NotImplementedException();
        }

    }
}
