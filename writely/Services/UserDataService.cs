using System.Threading.Tasks;
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

        public async Task<UserData> LoadUserData(string id)
        {
            var user = await _context.Users.FindAsync(id);
            
            return user == null ? null : new UserData(user);
        }
    }
}