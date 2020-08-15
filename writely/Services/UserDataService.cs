using writely.Data;
using writely.Models;
using writely.Models.Dto;

namespace writely.Services
{
    public class UserDataService : IUserDataService
    {
        private ApplicationDbContext _context;

        public UserDataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public UserData LoadUserData(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}