﻿using System;
namespace writely.Models.Dto
{
    public class UserRegistrationDto
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        public UserRegistrationDto()
        {
        }
    }
}
