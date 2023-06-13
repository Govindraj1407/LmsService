using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ViewModels;

namespace Services
{
    public interface IUserService
    {
        public Task<string> CreateUser(User user);
        public Task<string> UpdateUser(User user);

        public Task<string> DeleteUser(int userId);

        public Task<IEnumerable<User>> GetAllUser();

        public Task<User> GetUser(int id);
    }
}
