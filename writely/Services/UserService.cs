using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> Register(UserRegistrationDto registration)
        {
            if (registration.Password != registration.ConfirmPassword)
            {
                return IdentityResult.Failed();
            }

            var user = await _userManager.FindByEmailAsync(registration.Email);
            if (user != null)
            {
                return IdentityResult.Failed(GenerateError("Email already registered"));
            }
            
            var newUser = new AppUser(registration);
            return await _userManager.CreateAsync(newUser, registration.Password);
        }

        public async Task<IdentityResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(GenerateError("Error deleting account"));
            }
            
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> DisableAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return IdentityResult.Failed(GenerateError("Unable to locate user"));
            }
            
            user.IsAccountActive = false;
            return await _userManager.UpdateAsync(user);
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
