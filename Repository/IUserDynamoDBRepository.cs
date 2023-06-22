using Amazon.DynamoDBv2.DocumentModel;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public interface IUserDynamoDBRepository
    {
        public Task<User> GetUser(string id);

        public Task<IEnumerable<User>> GetAllUser();

        public Task<string> CreateUser(User user);

        public Task<string> UpdateUser(User user);

        public Task<string> DeleteUser(string userId);
    }
}
