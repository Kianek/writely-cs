using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using writely.Exceptions;
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
                throw new PasswordMismatchException("Passwords must match");
            }

            var user = await _userManager.FindByEmailAsync(registration.Email);
            if (user != null)
            {
                throw new DuplicateEmailException($"Email already registered: {registration.Email}");
            }
            
            var newUser = new AppUser(registration);
            return await _userManager.CreateAsync(newUser, registration.Password);
        }

        public async Task<UserDto> GetSignedInUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException($"User not found: {email}");
            }
            
            return new UserDto(user);
        }

        public async Task<IdentityResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"User not found: {id}");
            }
            
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ActivateAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"User not found: {id}");
            }
            
            user.IsAccountActive = true;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DisableAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"User not found: {id}");
            }
            
            user.IsAccountActive = false;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePassword(string id, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException($"User not found: {id}");
            }
            
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<UserData> GetUserData([FromServices] IUserDataService dataService, string id)
        {
            try
            {
                return await dataService.LoadUserData(id);
            }
            catch (UserNotFoundException ex)
            {
                Exception notFoundException = new Exception(ex.Message, ex);
                throw notFoundException;
            }
        }
    }
}
