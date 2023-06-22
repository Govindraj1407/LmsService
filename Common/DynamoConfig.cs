using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class DynamoConfig
    {
        public string DynamoUserKeyName { get; set; }
        public string DynamoUserTableName { get; set; }
        public string DynamoCourseKeyName { get; set; }
        public string DynamoCourseTableName { get; set; }
        public string DynamoUserCourseKeyName { get; set; }
        public string DynamoUserCourseTableName { get; set; }
        public string DynamoRegionName { get; set; }
        public string DynamoKeyType { get; set; }
        public string AwsKeyId { get; set; }
        public string AwsKey { get; set; }
    }
}
