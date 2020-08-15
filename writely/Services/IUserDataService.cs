using writely.Models.Dto;

namespace writely.Services
{
    public interface IUserDataService
    {
        UserData LoadUserData(string id);
    }
}