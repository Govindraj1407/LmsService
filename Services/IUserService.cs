using Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels;

namespace Elms.Services
{
    public interface IUserService
    {
        public Task<string> CreateUser(User user);
        public Task<string> UpdateUser(User user);

        public Task<string> DeleteUser(string userId);

        public Task<IEnumerable<UserViewModel>> GetAllUser();

        public Task<UserViewModel> GetUser(string id);

        public Task<UserViewModel> VerifyLogin(string userName, string password);
    }
}
