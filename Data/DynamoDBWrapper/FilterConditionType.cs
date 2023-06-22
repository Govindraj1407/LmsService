// <copyright file="FilterConditionType.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
    /// <summary>
    /// Represents the Filter Condition Type
    /// </summary>
    public enum FilterConditionType
    {
        AttributeWithoutOperator,
        AttributeWithOperatorAndAttributeValues,
        AttributeWithOperatorAndValues
    }
}
