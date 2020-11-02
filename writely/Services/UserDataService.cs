using System.Threading.Tasks;
using writely.Data;
using writely.Exceptions;
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
            if (user == null)
            {
                throw new UserNotFoundException($"User not found: {id}");
            }
            
            return new UserData(user);
        }
    }
}