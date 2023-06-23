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

        public string AttributeName { get; set; }

        public ScanOperator ScanOperator { get; set; }

        public Condition Condition { get; set; }

        public DynamoDBEntry[] DynamoDBEntry { get; set; }

        public List<AttributeValue> AttributeValues { get; set; }

        public FilterConditionType Type { get; set; }
    }
}
