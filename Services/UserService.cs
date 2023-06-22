using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DynamoDBWrapper;
using Common;
using Models;
using Repository;
using AutoMapper;
using ViewModels;

namespace Elms.Services
{
    public class UserService: IUserService
    {
        /// <summary>
        /// Gets or sets dynamo DB user repository
        /// </summary>
        private readonly IUserDynamoDBRepository dynamoDBUserRepository;        

        /// <summary>
        /// Mapper
        /// </summary>
        private readonly IMapper mapper;

        public UserService(IUserDynamoDBRepository dynamoDBUserRepository, IMapper mapper)
        {
            this.dynamoDBUserRepository = dynamoDBUserRepository;
            this.mapper = mapper;
        }

        public async Task<UserViewModel> GetUser(string id)
        {
            try
            {
                var user = await this.dynamoDBUserRepository.GetUser(id);
                UserViewModel userViewModel = this.mapper.Map<User, UserViewModel>(user);
                return userViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user", ex);
            }

        }

        public async Task<UserViewModel> VerifyLogin(string userName, string password)
        {
            try
            {
                var users = await this.dynamoDBUserRepository.GetAllUser();
                var user = users.Where(x => x.Name == userName);
                if (user.Any() && SecurePasswordHasher.Verify(password, user.First().Password))
                {
                    return await this.GetUser(user.First().UserId);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user", ex);
            }

        }

        public async Task<IEnumerable<UserViewModel>> GetAllUser()
        {
            try
            {
                var users = await this.dynamoDBUserRepository.GetAllUser();
                IEnumerable<UserViewModel> usersViewModel = this.mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users);
                return usersViewModel;
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
                user.UserId = Guid.NewGuid().ToString();
                user.Password = SecurePasswordHasher.Hash(user.Password);
                return await this.dynamoDBUserRepository.CreateUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when create a user", ex);
            }

        }

        public async Task<string> UpdateUser(User user)
        {
            try
            {
                user.Password = SecurePasswordHasher.Hash(user.Password);
                return await this.dynamoDBUserRepository.UpdateUser(user);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when update a user", ex);
            }

        }

        public async Task<string> DeleteUser(string userId)
        {
            try
            {
                return await this.dynamoDBUserRepository.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when delete a user", ex);
            }

        }
    }
}
