// <copyright file="ITableProxy.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// This thin wrapper around <seealso cref="Table">Table</seealso> class from the AWS DynamoDB SDK
   /// </summary>
   public interface ITableProxy
   {
      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation.
      /// </summary>
      /// <param name="document">Attributes to update.</param>
      /// <param name="config">Configuration to use.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task<Document> UpdateItemAsync(Document document, UpdateItemOperationConfig config);

      /// <summary>
      /// Initiates the asynchronous execution of the Query operation.
      /// </summary>
      /// <param name="config">Configuration to use.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      ISearch Query(QueryOperationConfig config);

      /// <summary>
      /// Initiates the asynchronous execution of the Scan operation.
      /// </summary>
      /// <param name="config">Configuration to use.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      ISearch Scan(ScanOperationConfig config);

      /// <summary>
      /// Creates a DocumentBatchGet object for the current table, allowing a batch-get
      ///     operation against DynamoDB.
      /// </summary>>
      /// <returns>
      ///     Empty DocumentBatchGet object.
      /// </returns>
      IDocumentBatchGet CreateBatchGet();

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
      /// </summary>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      IDocumentBatchWrite CreateBatchWrite();

      /// <summary>
      ///     Creates a DocumentBatchWrite object for the current table, allowing a batch-put/delete
      ///     operation against DynamoDB.
      /// </summary>      
      /// <param name="hashKey">Hash key element of the document.</param>
      /// <param name="rangeKey">Range key element of the document.</param>
      /// <returns>Empty DocumentBatchWrite object.</returns>
      Task<Document> DeleteItemAsync(Primitive hashKey, Primitive rangeKey);

      /// <summary>
      /// Initiates the asynchronous execution of the DeleteItem operation. Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem
      /// </summary>
      /// <param name="hashKey">Hash key element of the document.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task<Document> DeleteItemAsync(Primitive hashKey);

      /// <summary>
      /// Initiates the asynchronous execution of the PutItem operation. Amazon.DynamoDBv2.DocumentModel.Table.PutItem
      /// </summary>
      /// <param name="doc">Document to save.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task PutItemAsync(Document doc);

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
      /// </summary>      
      /// <param name="hashKey">Hash key element of the document.</param>
      /// <param name="config">Configuration to use.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task<Document> GetItemAsync(Primitive hashKey, GetItemOperationConfig config);

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
      /// </summary>
      /// <param name="hashKey">Hash key element of the document.</param>
      /// <param name="rangeKey">Range key element of the document.</param>
      /// <param name="config">Configuration to use.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task<Document> GetItemAsync(Primitive hashKey, Primitive rangeKey, GetItemOperationConfig config);
   }
}
