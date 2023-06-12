// <copyright file="IDocumentBatchGet.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// This thin wrapper around DocumentBatchGet class from the AWS DynamoDB SDK
   /// <seealso cref="DocumentBatchGet"/>
   /// </summary>
   public interface IDocumentBatchGet
   {
      /// <summary>
      /// The results of the ExecuteAsync
      /// </summary>
      List<Document> Results { get; }

      /// <summary>
      /// If set to true, a consistent read is issued.Otherwise eventually-consistent is used.
      /// </summary>
      bool ConsistentRead { get; set; }

      /// <summary>
      /// List of attributes to retrieve.
      /// </summary>
      List<string> AttributesToGet { get; set; }

      /// <summary>
      ///  Add a single item to get, identified by its hash-and-range primary key.
      /// </summary>
      /// <param name="hashKey">The hash key of the item to get</param>
      /// <param name="rangeKey">The range key element of the item to get</param>
      void AddKey(Primitive hashKey, Primitive rangeKey);

      /// <summary>
      /// Add a single item to get, identified by its hash primary key.
      /// </summary>
      /// <param name="hashKey">Hash key element of the item to get.</param>
      void AddKey(Primitive hashKey);

      /// <summary>
      /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
      /// </summary>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task ExecuteAsync();
   }
}
