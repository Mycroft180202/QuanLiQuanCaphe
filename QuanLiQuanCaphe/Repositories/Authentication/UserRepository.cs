using Microsoft.EntityFrameworkCore;
using QuanLiQuanCaphe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCaphe.Repositories.Authentication
{
    public class UserRepository
    {
        private readonly CoffeeShopManagementContext _context;

        public UserRepository(CoffeeShopManagementContext context)
        {
            _context = context;
        }

        public User GetUserByUsernameAndPassword(string username, string passwordHash)
        {
            return _context.Users.SingleOrDefault(user => user.Username == username && user.PasswordHash == passwordHash);
        }
        public void AddUser(User newUser)
        {
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public void DeleteUser(int userId)
        {
            var user = _context.Users.Find(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
        public List<User> GetAllUsers()
        {
            return _context.Users.Include(u => u.Role).ToList();
        }
    }
}
