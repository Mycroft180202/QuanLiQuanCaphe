using System.Linq;
using QuanLiQuanCaphe.Models;

namespace QuanLiQuanCaphe.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly CoffeeShopManagementContext _context;

        public AuthenticationService(CoffeeShopManagementContext context)
        {
            _context = context;
        }

        public User Authenticate(string username, string passwordHash)
        {
            return _context.Users.SingleOrDefault(user => user.Username == username && user.PasswordHash == passwordHash);
        }
    }
}
