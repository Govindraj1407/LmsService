// <copyright file="IDocumentBatchWrite.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// This thin wrapper around <seealso cref="DocumentBatchWrite">DocumentBatchGet </seealso> from the AWS DynamoDB SDK
   /// </summary>
   public interface IDocumentBatchWrite
   {
      /// <summary>
      /// Add a single Document to put.
      /// </summary>
      /// <param name="doc">Document to put.</param>
      void AddDocumentToPut(Document doc);

      /// <summary>
      /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
      /// </summary>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task ExecuteAsync();

      /// <summary>
      /// Add a single item to delete, identified by its hash primary key.
      /// </summary>
      /// <param name="hashKey">Hash key element of the item to delete.</param>
      /// <param name="rangeKey">Range key element of the item to delete.</param>
      void AddKeyToDelete(Primitive hashKey, Primitive rangeKey);

      /// <summary>
      /// Add a single item to delete, identified by its hash primary key.
      /// </summary>
      /// <param name="hashKey">Hash key element of the item to delete.</param>
      void AddKeyToDelete(Primitive hashKey);
   }
}
