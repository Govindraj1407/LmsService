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
    public class CourseWareDynamoDBRepository : ICourseWareDynamoDBRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CourseWareDynamoDBRepository"/> class.
        /// </summary>
        /// <param name="logger">Defines methods that captures the logging messages</param>
        /// <param name="appSettings">App settings for fetching environment details</param>
        /// <param name="dynamoTableConfig">Dynamo table details</param>
        public CourseWareDynamoDBRepository(ILogger<DynamoDBRepository> logger, IOptions<DynamoConfig> appSettings, IDynamoTableConfig dynamoTableConfig)
        {
            this.Logger = logger;
            this.DynamoTableConfig = dynamoTableConfig;

            dynamoTableConfig.TableName = appSettings.Value.DynamoCourseTableName;
            dynamoTableConfig.KeyName = appSettings.Value.DynamoCourseTableName;
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

        public async Task<Course> GetCourse(string id)
        {
            try
            {
                var course = await this.dynamoDBRepository.GetAsync<Course>(id);
                return course;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get a course", ex);
            }

        }

        public async Task<IEnumerable<Course>> GetAllCourse()
        {
            try
            {
                var conditions = new List<ScanFilterCondition>();
                var courses = await this.dynamoDBRepository.ScanAsync<Course>(conditions);
                return courses;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occured when get all courses", ex);
            }
        }

        public async Task<string> CreateCourse(Course course)
        {
            try
            {
                await this.dynamoDBRepository.InsertAsync(course);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "Course creation failed. Message:" + exception.Message;
            }

        }

        public async Task<string> UpdateCourse(Course course)
        {
            try
            {
                await this.dynamoDBRepository.PartialUpdateCommandAsync(course);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "Course update failed. Message:" + exception.Message;
            }

        }

        public async Task<string> DeleteCourse(string courseId)
        {
            try
            {
                await this.dynamoDBRepository.DeleteAsync(courseId);
                return string.Empty;
            }
            catch (Exception exception)
            {
                return "Course update failed. Message:" + exception.Message;
            }

        }
    }
}
