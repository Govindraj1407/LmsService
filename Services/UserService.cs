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
        ///// <summary>
        ///// Pricing document db repository
        ///// </summary>
        //private readonly IUserDocumentDbRepository userDocumentDbRepository;

        /// <summary>
        /// Gets or sets dynamo DB repository
        /// </summary>
        private readonly IDynamoDBRepository<string, User> dynamoDBRepository;

        public UserService(IDynamoRepositoryFactory factory)
        {
            this.dynamoDBRepository = factory.Get<string, User>(
                "User",
                "UserId");
        }

        public async Task<string> CreateUser(User user)
        {
            await this.dynamoDBRepository.InsertAsync(user);
            //if(user == null)
            //{
            //    return false;
            //}
            //string json = JsonSerializer.Serialize(user);
            //File.WriteAllText(@"C:\userinfopath.json", json);
            return "Saved";
        }
    }
}
