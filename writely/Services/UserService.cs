using System;
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

        public int Register(UserRegistrationDto registration)
        {
            throw new NotImplementedException();
        }

        public int DeleteAccount(string id)
        {
            throw new NotImplementedException();
        }

        public int DisableAccount(string id)
        {
            throw new NotImplementedException();
        }

        public UserData GetUserData(string id)
        {
            throw new NotImplementedException();
        }

    }
}
