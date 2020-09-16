using System;
using System.Collections.Generic;

namespace writely.Models.Dto
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public UserDto()
        {
        }

        public UserDto(AppUser user)
        {
            if (user == null)
            {
                throw new NullReferenceException("AppUser reference is null");
            }
            
            Id = user.Id;
            Username = user.UserName;
            Email = user.Email;
        }
    }
}
