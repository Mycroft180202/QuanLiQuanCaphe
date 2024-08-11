using QuanLiQuanCaphe.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCaphe.Repositories.Authentication
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
        User GetUserByUsername(string username);
        void AddUser(User newUser);
        User GetUserByUsernameAndPassword(string username, string password);
    }
}
