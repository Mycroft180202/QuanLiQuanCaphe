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
        void DeleteUser(User user);
        void GetUserByUsernameAndPassword(string username, string password);
    }
}
