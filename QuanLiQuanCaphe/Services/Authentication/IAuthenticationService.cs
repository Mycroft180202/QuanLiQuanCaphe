using QuanLiQuanCaphe.Models;

namespace QuanLiQuanCaphe.Services
{
    public interface IAuthenticationService
    {
        
        User? Authenticate(string username, string password);
    }
}
