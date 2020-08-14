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

        public async Task<IdentityResult> Register(UserRegistrationDto registration)
        {
            if (registration.Password != registration.ConfirmPassword)
            {
                return IdentityResult.Failed();
            }

            var user = await userManager.FindByEmailAsync(registration.Email);
            if (user != null)
            {
                return IdentityResult.Failed(GenerateError("Email already registered"));
            }
            
            var newUser = new AppUser(registration);
            return await userManager.CreateAsync(newUser, registration.Password);
        }

        public async Task<IdentityResult> DeleteAccount(string id)
        {
            throw new NotImplementedException();
        }

        public async  Task<IdentityResult> DisableAccount(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserData> GetUserData(string id)
        {
            throw new NotImplementedException();
        }

        private IdentityError GenerateError(string description = "")
        {
            return new IdentityError
            {
                Description = description
            };
        }
    }
}
