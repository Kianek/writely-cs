using System.Threading.Tasks;
using writely.Models.Dto;

namespace writely.Services
{
    public interface IUserDataService
    {
        Task<UserData> LoadUserData(string id);
    }
}