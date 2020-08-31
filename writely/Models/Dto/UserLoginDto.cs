using System;
namespace writely.Models.Dto
{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public UserLoginDto()
        {
        }
    }
}
