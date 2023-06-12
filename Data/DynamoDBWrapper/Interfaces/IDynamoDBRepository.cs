// <copyright file="IDynamoDBRepository.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// Implementation for accessing Amazon DynamoDB for various CRUD operations such as Get, Put, Delete, etc.,
   /// </summary>
   public interface IDynamoDBRepository<Key, Item>
      where Item : new()
   {
      /// <summary>
      /// Initiates the asynchronous execution of the PutItem operation. Amazon.DynamoDBv2.DocumentModel.Table.PutItem
      /// </summary>      
      /// <param name="tObject">Document to save</param>
      /// <returns>List of attribute key-value pairs that defines an item in DynamoDB.</returns>
      Task InsertAsync(Item tObject);

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation.
      /// </summary>
      /// <param name="id">The partition key of an item</param>
      /// <param name="attribute">Each attribute value is described as a name-value pair. The name is the data type, and the value is the data itself.</param>
      /// <param name="value">Represents the data for an attribute.</param>
      /// <returns>Output of an UpdateItem operation.</returns>
      Task UpdateAsync(Key id, string attribute, string value);

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation.
      /// </summary>
      /// <param name="id">The partition key of an item</param>
      /// <param name="sortKey">The sort key of an item</param>
      /// <param name="attribute">Each attribute value is described as a name-value pair. The name is the data
      ///     type, and the value is the data itself.</param>
      /// <param name="value">Represents the data for an attribute.</param>
      /// <returns>Output of an UpdateItem operation.</returns>
      Task UpdateAsync(Key id, object sortKey, string attribute, string value);

      /// <summary>
      /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
      /// </summary>
      /// <param name="id">The partition key of an item</param>
      /// <param name="sortKey">The sort key of an item</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">IEnumerable of attributes to retrieve</param>
      /// <returns>A collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      Task<Item> GetAsync(Key id, object sortKey = null, bool consistentRead = true, IEnumerable<string> attributesToGet = null);

      /// <summary>
      /// Initiates the asynchronous execution of the DeleteItem operation. Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem
      /// </summary>
      /// <param name="id">The partition key of an item</param>
      /// <param name="sortKey">The sort key of an item</param>
      /// <returns>Status of the Async operation</returns>
      Task DeleteAsync(Key id, object sortKey = null);

      /// <summary>
      /// Initiates the asynchronous execution of the Batch Insert operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
      /// </summary>
      /// <param name="batchOfObjects">One or more documents to save.</param>
      /// <returns>Status of the batch insert operation</returns>
      Task BatchInsertAsync(IEnumerable<Item> batchOfObjects);

      /// <summary>
      /// Initiates the asynchronous execution of the Batch Delete operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
      /// </summary>
      /// <param name="idList">IEnumerable of partition keys</param>
      /// <param name="sortKeyList">IEnumerable of sort keys</param>
      /// <returns>Status of the batch delete operation.</returns>
      Task BatchDeleteAsync(IEnumerable<Key> idList, IEnumerable<object> sortKeyList = null);

      /// <summary>
      /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
      /// </summary>
      /// <param name="idList">IEnumerable of partition keys</param>
      /// <param name="sortKeyList">IEnumerable of sort keys</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">List of attributes to retrieve</param>
      /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      Task<List<Item>> BatchGetAsync(IEnumerable<Key> idList, IEnumerable<object> sortKeyList = null, bool consistentRead = true, IEnumerable<string> attributesToGet = null);

      /// <summary>
      /// Initiates the asynchronous execution of the GetNextSet operation. If there are
      ///     more items in the Scan/Query, PaginationToken will be set and can be consumed
      ///     in a new Scan/Query operation to resume retrieving items from this point.
      ///     /// Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
      /// </summary>
      /// <param name="scanFilterConditions">Represents the selection criteria for a Query or Scan operation</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">List of attributes to retrieve</param>
      /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      Task<List<Item>> ScanAsync(IEnumerable<ScanFilterCondition> scanFilterConditions, bool consistentRead = true, IEnumerable<string> attributesToGet = null);

      /// <summary>
      /// Initiates the asynchronous execution of the GetNextSet operation. If there are
      ///     more items in the Scan/Query, PaginationToken will be set and can be consumed
      ///     in a new Scan/Query operation to resume retrieving items from this point. Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
      /// </summary>      
      /// <param name="queryFilterConditions">Represents the selection criteria for a Query or Scan operation</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">List of attributes to retrieve</param>
      /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      Task<List<Item>> QueryAsync(IEnumerable<QueryFilterCondition> queryFilterConditions, bool consistentRead = true, IEnumerable<string> attributesToGet = null);

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
      /// </summary>      
      /// <param name="tObject">Attributes to update.</param>
      /// <param name="returnValues">Flag specifying what values should be returned.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task<Item> PartialUpdateCommandAsync(Item tObject, ReturnValues returnValues = ReturnValues.AllNewAttributes);
   }
}
