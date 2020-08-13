using System;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public interface IUserService
    {
        int Register(UserRegistrationDto registration);
        int DeleteAccount(string id);
        int DisableAccount(string id);
        UserData GetUserData(string id);
    }
}
