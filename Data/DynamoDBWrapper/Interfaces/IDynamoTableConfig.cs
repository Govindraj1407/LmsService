// <copyright file="IDynamoTableConfig.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
    using System;

    /// <summary>
    /// Interface for configuring the specifications of the Dynamo Table
    /// </summary>
    public interface IDynamoTableConfig
    {
        /// <summary>
        /// Gets or sets the Name of the Dynamo DB table.
        /// </summary>
        string TableName { get; set; }

        /// <summary>
        /// Gets or sets the Type of the partition key
        /// </summary>
        Type KeyType { get; set; }

        /// <summary>
        /// Gets or sets the Name of the partition key
        /// </summary>
        string KeyName { get; set; }

        /// <summary>
        /// Gets or sets the Type of the range key
        /// </summary>
        Type SortKeyType { get; set; }

        /// <summary>
        /// Gets or sets the Name of the range key
        /// </summary>
        string SortKeyName { get; set; }

        /// <summary>
        /// Gets or sets the The maximum number of strongly consistent reads consumed per second before DynamoDB returns a ThrottlingException
        /// </summary>
        int ReadCapacityUnits { get; set; }

        /// <summary>
        /// Gets or sets the The maximum number of writes consumed per second before DynamoDB returns a ThrottlingException
        /// </summary>
        int WriteCapacityUnits { get; set; }

        /// <summary>
        /// Gets or sets the Name of the index to query/scan against.
        /// </summary>
        string IndexName { get; set; }
    }
}
