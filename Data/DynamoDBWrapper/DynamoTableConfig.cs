namespace DynamoDBWrapper
{
    using System;

    /// <summary>
    /// Implementation for configuring the specifications of the Dynamo Table
    /// </summary>
    public class DynamoTableConfig : IDynamoTableConfig
    {
        /// <summary>
        /// Gets or sets the Name of the Dynamo DB table.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets the Type of the partition key
        /// </summary>
        public Type KeyType { get; set; }

        /// <summary>
        /// Gets or sets the Name of the partition key
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// Gets or sets the Type of the range key
        /// </summary>
        public Type SortKeyType { get; set; }

        /// <summary>
        /// Gets or sets the Name of the range key
        /// </summary>
        public string SortKeyName { get; set; }

        /// <summary>
        /// Gets or sets the The maximum number of strongly consistent reads consumed per second before DynamoDB returns a ThrottlingException
        /// </summary>
        public int ReadCapacityUnits { get; set; }

        /// <summary>
        /// Gets or sets the The maximum number of writes consumed per second before DynamoDB returns a ThrottlingException
        /// </summary>
        public int WriteCapacityUnits { get; set; }

        /// <summary>
        /// Gets or sets the Name of the index to query/scan against.
        /// </summary>
        public string IndexName { get; set; }
    }
}
