// <copyright file="FilterConditionType.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
    /// <summary>
    /// Represents the Filter Condition Type
    /// </summary>
    public enum FilterConditionType
    {
        /// <summary>
        /// Attribute Without Operator
        /// </summary>
        AttributeWithoutOperator,

        /// <summary>
        /// Attribute With Operator And Attribute Values
        /// </summary>
        AttributeWithOperatorAndAttributeValues,

        /// <summary>
        /// Attribute With Operator And Values
        /// </summary>
        AttributeWithOperatorAndValues
    }
}
