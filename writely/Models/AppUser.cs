using System;
using Microsoft.AspNetCore.Identity;
using writely.Data;
using writely.Models.Dto;

namespace writely.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAccountActive { get; set; }

        public AppUser()
        {
        }

        public AppUser(UserRegistrationDto user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            IsAccountActive = false;
            base.Email = user.Email;
            base.UserName = user.Username;
        }
    }
}
