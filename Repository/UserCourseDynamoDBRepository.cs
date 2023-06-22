using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
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
    public class UserCourseDynamoDBRepository : IUserCourseDynamoDBRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobsUpdateDynamoDBRepository"/> class.
        /// </summary>
        /// <param name="logger">Defines methods that captures the logging messages</param>
        /// <param name="dynamoTableConfig">Dynamo table details</param>
        /// <param name="appSettings">App settings for fetching environment details</param>
        public UserCourseDynamoDBRepository(ILogger<DynamoDBRepository> logger, IDynamoTableConfig dynamoTableConfig, IOptions<DynamoConfig> appSettings)
        {
            this.Logger = logger;
            this.DynamoTableConfig = dynamoTableConfig;

            dynamoTableConfig.TableName = appSettings.Value.DynamoUserCourseTableName;
            dynamoTableConfig.KeyName = appSettings.Value.DynamoUserCourseKeyName;
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

        public async Task<IEnumerable<UserCourse>> GetUserCourses(string userId)
        {
            try
            {
                var conditions = new List<ScanFilterCondition>();
                Condition condtion = new Condition();
                condtion.AttributeValueList = new List<AttributeValue>() { new AttributeValue(userId)};
                condtion.ComparisonOperator = ComparisonOperator.EQ;
                conditions.Add(new ScanFilterCondition("UserId", condtion));
                var userCourses = await this.dynamoDBRepository.ScanAsync<UserCourse>(conditions);
                return userCourses;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user course", ex);
            }
        }

        public async Task<IEnumerable<UserCourse>> GetAllUserCourses()
        {
            try
            {
                var conditions = new List<ScanFilterCondition>();
                var userCourses = await this.dynamoDBRepository.ScanAsync<UserCourse>(conditions);
                return userCourses;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user course", ex);
            }
        }

        public async Task<string> AddUserCourse(UserCourse userCourse)
        {
            try
            {
                await this.dynamoDBRepository.InsertAsync<UserCourse>(userCourse);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a user course", ex);
            }
        }
    }
}
