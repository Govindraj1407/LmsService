// <copyright file="IDynamoClient.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynamoDBWrapper
{
   /// <summary>
   /// Thin wrapper around <see cref="Amazon.DynamoDBv2.DocumentModel.Table"/>
   /// </summary>
   public interface IDynamoClient
   {
      /// <summary>
      /// Initiates the asynchronous execution of the PutItem operation. Amazon.DynamoDBv2.DocumentModel.Table.PutItem
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="doc">The Document to save.</param>
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task PutItemAsync(string tableName, Document doc);

      /// <summary>
      /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
      /// </summary>      
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="hashKey">Hash key element of the document.</param>
      /// <param name="config">Configuration to use.</param>      
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task<Document> GetItemAsync(string tableName, Primitive hashKey, GetItemOperationConfig config);

      /// <summary>
      /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="hashKey">Hash key element of the document.</param>
      /// <param name="rangeKey">Range key element of the document.</param>
      /// <param name="config">Configuration to use.</param>      
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task<Document> GetItemAsync(string tableName, Primitive hashKey, Primitive rangeKey, GetItemOperationConfig config);

      /// <summary>
      /// Initiates the asynchronous execution of the DeleteItem operation. Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="hashKey">Hash key element of the document.</param>
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task<Document> DeleteItemAsync(string tableName, Primitive hashKey);

      /// <summary>
      /// Initiates the asynchronous execution of the DeleteItem operation. Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="hashKey">Hash key element of the document.</param>
      /// <param name="rangeKey">Range key element of the document.</param>
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task<Document> DeleteItemAsync(string tableName, Primitive hashKey, Primitive rangeKey);

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="document"></param>
      /// <param name="config">Configuration to use.</param>
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task<Document> UpdateItemAsync(string tableName, Document document, UpdateItemOperationConfig config);

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="request">The request detailing what exactly we're saving.</param>      
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task UpdateItemAsync(string tableName, UpdateItemRequest request);

      /// <summary>
      /// Initiates the asynchronous execution of the BatchInsert operation. Amazon.DynamoDBv2.DocumentModel.Table.BatchInsert
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="documents">The documents to insert.</param>      
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task BatchInsertAsync(string tableName, IEnumerable<Document> documents);

      /// <summary>
      /// Initiates the asynchronous execution of the BatchDelete operation.
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="hashKeys">List of Hash key element of the documents to delete.</param>
      /// <param name="rangeKeys">List of Range key element of the documents to delete.</param>
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task BatchDeleteAsync(string tableName, List<object> hashKeys, List<object> rangeKeys);

      /// <summary>
      /// Initiates the asynchronous execution of the BatchGet operation.
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="hashKeys">List of Hash key element of the documents to delete.</param>
      /// <param name="rangeKeys">List of Range key element of the documents to delete.</param>
      /// <param name="consistentRead"></param>
      /// <param name="attributesToGet"></param>      
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task<List<Document>> BatchGetAsync(string tableName, List<object> hashKeys, List<object> rangeKeys, bool consistentRead, IEnumerable<string> attributesToGet);

      /// <summary>
      ///  Initiates a Search object to Scan a DynamoDB table, with the specified config.		
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="config">Configuration to use.</param>
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task<List<T>> ScanAsync<T>(string tableName, ScanOperationConfig config);

      /// <summary>
      /// Initiates a Search object to Query a DynamoDB table, with the specified config.		
      /// </summary>
      /// <param name="tableName">The table to insert the item into.</param>
      /// <param name="config">Configuration to use.</param>      
      /// <returns>A task that can be used to poll or wait for results, or both.</returns>
      Task<List<T>> QueryAsync<T>(string tableName, QueryOperationConfig config);
   }
}
