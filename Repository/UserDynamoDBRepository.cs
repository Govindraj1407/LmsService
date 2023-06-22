using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Common;
using DynamoDBWrapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository
{
    public class UserDynamoDBRepository : IUserDynamoDBRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsUpdateDynamoDBRepository"/> class.
        /// </summary>
        /// <param name="logger">Defines methods that captures the logging messages</param>
        /// <param name="dynamoTableConfig">Dynamo table details</param>
        /// <param name="appSettings">App settings for fetching environment details</param>
        public UserDynamoDBRepository(ILogger<DynamoDBRepository> logger, IDynamoTableConfig dynamoTableConfig, IOptions<DynamoConfig> appSettings)
        {
            this.Logger = logger;
            this.DynamoTableConfig = dynamoTableConfig;

            dynamoTableConfig.TableName = appSettings.Value.DynamoUserTableName;
            dynamoTableConfig.KeyName = appSettings.Value.DynamoUserTableName;
            dynamoTableConfig.KeyType = typeof(string);

            var config = new AmazonDynamoDBConfig
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(appSettings.Value.DynamoRegionName)
            };
            var awsClient = new AmazonDynamoDBClient(appSettings.Value.AwsKeyId, appSettings.Value.AwsKey, config);

            this.dynamoDBRepository = new DynamoDBRepository(awsClient, dynamoTableConfig, this.Logger);
        }

        /// <summary>
        /// Gets or sets DynamoDBRepository
        /// </summary>
        public IDynamoDBRepository dynamoDBRepository { get; set; }

        /// <summary>
        /// Gets or sets Logger
        /// </summary>
        private ILogger<DynamoDBRepository> Logger { get; set; }

        /// <summary>
        /// Gets or sets DynamoTableConfig
        /// </summary>
        private IDynamoTableConfig DynamoTableConfig { get; set; }

        public async Task<User> GetUser(string id)
        {
            try
            {
                var attributesToGet = new List<string> { "UserId", "Name", "Role", "City", "State", "Phone", "Pin", "Email" };
                var user = await this.dynamoDBRepository.GetAsync<User>(id, null, true, attributesToGet);
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
                var users = await this.dynamoDBRepository.ScanAsync<User>(conditions);
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

        public async Task<string> DeleteUser(string userId)
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
