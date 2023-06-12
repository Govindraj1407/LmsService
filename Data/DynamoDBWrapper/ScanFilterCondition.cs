// <copyright file="ScanFilterCondition.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
    using System.Collections.Generic;
    using Amazon.DynamoDBv2.DocumentModel;
    using Amazon.DynamoDBv2.Model;

    /// <summary>
    /// Represents the selection criteria for a Scan operation
    /// </summary>
    public class ScanFilterCondition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFilterCondition"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="condition">Represents the selection criteria for a Query or Scan operation</param>
        public ScanFilterCondition(string attributeName, Condition condition)
        {
            this.AttributeName = attributeName;
            this.Condition = condition;
            this.Type = FilterConditionType.AttributeWithoutOperator;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFilterCondition"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="scanOperator">An enumeration of all supported scan operator directives</param>
        /// <param name="attributeValues">Represents the data for attribute</param>
        public ScanFilterCondition(string attributeName, ScanOperator scanOperator, List<AttributeValue> attributeValues)
        {
            this.AttributeName = attributeName;
            this.Type = FilterConditionType.AttributeWithOperatorAndAttributeValues;
            this.ScanOperator = scanOperator;
            this.AttributeValues = attributeValues;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanFilterCondition"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute</param>
        /// <param name="scanOperator">An enumeration of all supported scan operator directives</param>
        /// <param name="values">Represents DynamoDB attribue value</param>
        public ScanFilterCondition(string attributeName, ScanOperator scanOperator, params DynamoDBEntry[] values)
        {
            this.AttributeName = attributeName;
            this.Type = FilterConditionType.AttributeWithOperatorAndValues;
            this.ScanOperator = scanOperator;
            this.DynamoDBEntry = values;
        }

        /// <summary>
        /// Gets or sets AttributeName
        /// </summary>
        public string AttributeName { get; set; }

        /// <summary>
        /// Gets or sets ScanOperator
        /// </summary>
        public ScanOperator ScanOperator { get; set; }

        /// <summary>
        /// Gets or sets Condition
        /// </summary>
        public Condition Condition { get; set; }

        /// <summary>
        /// Gets or sets DynamoDBEntry
        /// </summary>
        public DynamoDBEntry[] DynamoDBEntry { get; set; }

        /// <summary>
        /// Gets or sets AttributeValues
        /// </summary>
        public List<AttributeValue> AttributeValues { get; set; }

        /// <summary>
        /// Gets or sets Type
        /// </summary>
        public FilterConditionType Type { get; set; }
    }
}
