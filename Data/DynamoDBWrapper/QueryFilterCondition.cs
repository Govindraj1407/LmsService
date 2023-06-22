// <copyright file="QueryFilterCondition.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
    using System.Collections.Generic;
    using Amazon.DynamoDBv2.DocumentModel;
    using Amazon.DynamoDBv2.Model;

    /// <summary>
    /// Represents the selection criteria for a Query operation
    /// </summary>
    public class QueryFilterCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFilterCondition"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="condition">Represents the selection criteria for a Query or Scan operation</param>
        public QueryFilterCondition(string attributeName, Condition condition)
        {
            this.AttributeName = attributeName;
            this.Condition = condition;
            this.Type = FilterConditionType.AttributeWithoutOperator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFilterCondition"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="queryOperator">An enumeration of all supported query operator directives</param>
        /// <param name="attributeValues">Represents the data for attribute</param>
        public QueryFilterCondition(string attributeName, QueryOperator queryOperator, List<AttributeValue> attributeValues)
        {
            this.AttributeName = attributeName;
            this.Type = FilterConditionType.AttributeWithOperatorAndAttributeValues;
            this.QueryOperator = queryOperator;
            this.AttributeValues = attributeValues;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryFilterCondition"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="queryOperator">An enumeration of all supported scan operator directives</param>
        /// <param name="values">Represents DynamoDB attribue value</param>
        public QueryFilterCondition(string attributeName, QueryOperator queryOperator, params DynamoDBEntry[] values)
        {
            this.AttributeName = attributeName;
            this.Type = FilterConditionType.AttributeWithOperatorAndValues;
            this.QueryOperator = queryOperator;
            this.DynamoDBEntry = values;
        }

        public string AttributeName { get; set; }

        public QueryOperator QueryOperator { get; set; }

        public Condition Condition { get; set; }

        public DynamoDBEntry[] DynamoDBEntry { get; set; }

        public List<AttributeValue> AttributeValues { get; set; }

        public FilterConditionType Type { get; set; }
    }
}
