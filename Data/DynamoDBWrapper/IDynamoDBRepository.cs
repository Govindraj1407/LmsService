// <copyright file="IDynamoDBRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Amazon.DynamoDBv2.DocumentModel;

    /// <summary>
    /// Implementation for accessing Amazon DynamoDB for various CRUD operations such as Get, Put, Delete, etc.,
    /// </summary>
    public interface IDynamoDBRepository
    {
        /// <summary>
        /// Initiates the asynchronous execution of the PutItem operation. Amazon.DynamoDBv2.DocumentModel.Table.PutItem
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="tObject">Document to save</param>
        /// <returns>List of attribute key-value pairs that defines an item in DynamoDB.</returns>
        Task InsertAsync<T>(T tObject);

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// </summary>
        /// <param name="hashKey">The partition key of an item</param>
        /// <param name="attribute">Each attribute value is described as a name-value pair. The name is the data
        ///     type, and the value is the data itself.</param>
        /// <param name="value">Represents the data for an attribute.</param>
        /// <returns>Output of an UpdateItem operation.</returns>
        Task UpdateAsync(object hashKey, string attribute, string value);

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// </summary>
        /// <param name="hashKey">The partition key of an item</param>
        /// <param name="sortKey">The sort key of an item</param>
        /// <param name="attribute">Each attribute value is described as a name-value pair. The name is the data
        ///     type, and the value is the data itself.</param>
        /// <param name="value">Represents the data for an attribute.</param>
        /// <returns>Output of an UpdateItem operation.</returns>
        Task UpdateAsync(object hashKey, object sortKey, string attribute, string value);

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
        /// </summary>
        /// <param name="hashKey">The partition key of an item</param>
        /// <param name="sortKey">The sort key of an item</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>A collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        Task<Document> GetAsync(object hashKey, object sortKey = null, bool consistentRead = true, List<string> attributesToGet = null);

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="hashKey">The partition key of an item</param>
        /// <param name="sortKey">The sort key of an item</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>Generic list of attribute key-value pairs that defines an item in DynamoDB.</returns>
        Task<T> GetAsync<T>(object hashKey, object sortKey = null, bool consistentRead = true, List<string> attributesToGet = null);

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation. Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem
        /// </summary>
        /// <param name="id">The partition key of an item</param>
        /// <param name="sortKey">The sort key of an item</param>
        /// <returns>Status of the Async operation</returns>
        Task DeleteAsync(object id, object sortKey = null);

        /// <summary>
        /// Initiates the asynchronous execution of the Batch Insert operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="batchOfObjects">One or more documents to save.</param>
        /// <returns>Status of the batch insert operation</returns>
        Task BatchInsertAsync<T>(List<T> batchOfObjects);

        /// <summary>
        /// Initiates the asynchronous execution of the Batch Delete operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
        /// </summary>
        /// <param name="idList">List of partition keys</param>
        /// <param name="sortKeyList">List of sort keys</param>
        /// <returns>Status of the batch delete operation.</returns>
        Task BatchDeleteAsync(List<object> idList, List<object> sortKeyList = null);

        /// <summary>
        /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
        /// </summary>
        /// <param name="idList">List of partition keys</param>
        /// <param name="sortKeyList">List of sort keys</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        Task<List<Document>> BatchGetAsync(List<object> idList, List<object> sortKeyList = null, bool consistentRead = true, List<string> attributesToGet = null);

        /// <summary>
        /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="idList">List of partition keys</param>
        /// <param name="sortKeyList">List of sort keys</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        Task<List<T>> BatchGetAsync<T>(List<object> idList, List<object> sortKeyList = null, bool consistentRead = true, List<string> attributesToGet = null)
            where T : new();

        /// <summary>
        /// Initiates the asynchronous execution of the GetNextSet operation. If there are
        ///     more items in the Scan/Query, PaginationToken will be set and can be consumed
        ///     in a new Scan/Query operation to resume retrieving items from this point.
        ///     /// Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="scanFilterConditions">Represents the selection criteria for a Query or Scan operation</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        Task<List<T>> ScanAsync<T>(List<ScanFilterCondition> scanFilterConditions, bool consistentRead = true, List<string> attributesToGet = null);

        /// <summary>
        /// Initiates the asynchronous execution of the GetNextSet operation. If there are
        ///     more items in the Scan/Query, PaginationToken will be set and can be consumed
        ///     in a new Scan/Query operation to resume retrieving items from this point. Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="queryFilterConditions">Represents the selection criteria for a Query or Scan operation</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        Task<List<T>> QueryAsync<T>(List<QueryFilterCondition> queryFilterConditions, bool consistentRead = true, List<string> attributesToGet = null);

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="tObject">Attributes to update.</param>
        /// <param name="returnValues">Flag specifying what values should be returned.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        Task<Document> PartialUpdateCommandAsync<T>(T tObject, ReturnValues returnValues = ReturnValues.AllNewAttributes);
    }
}
