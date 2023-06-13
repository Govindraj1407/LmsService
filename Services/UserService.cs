using Core.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamoDBWrapper;
using ViewModels;

namespace Services
{
    public class UserService: IUserService
    {
        /// <summary>
        /// Gets or sets dynamo DB repository
        /// </summary>
        private readonly IDynamoDBRepository<int, User> dynamoDBRepository;

        public UserService(IDynamoRepositoryFactory factory)
        {
            this.dynamoDBRepository = factory.Get<int, User>(
                "User",
                "UserId");
        }
        public async Task<User> GetUser(int id)
        {
            try
            {
                var user = await this.dynamoDBRepository.GetAsync(id);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user", ex);
            }

        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            try
            {
                var conditions = new List<ScanFilterCondition>();
                var users = await this.dynamoDBRepository.ScanAsync(conditions);
                return users;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get all users", ex);
            }
        }

        public async Task<string> CreateUser(User user)
        {
            try
            {
                await this.dynamoDBRepository.InsertAsync(user);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "User creation failed. Message:" + exception.Message;
            }

        }

        public async Task<string> UpdateUser(User user)
        {
            try
            {
                await this.dynamoDBRepository.PartialUpdateCommandAsync(user);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "User update failed. Message:" + exception.Message;
            }

        }

        public async Task<string> DeleteUser(int userId)
        {
            try
            {
                await this.dynamoDBRepository.DeleteAsync(userId);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "User update failed. Message:" + exception.Message;
            }

        }
    }
}
