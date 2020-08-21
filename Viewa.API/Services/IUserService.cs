
using System.Threading.Tasks;
using Viewa.Db;
using Viewa.Models;

namespace Viewa.Services
{
    public interface IUserService
    {
        Task<LoginResponse> Login(string username, string password);
        Task<UserData> GetUserData(string username);
    }
}
