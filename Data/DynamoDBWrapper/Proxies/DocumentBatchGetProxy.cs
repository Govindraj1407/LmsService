// <copyright file="DocumentBatchGetProxy.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// Thin wrapper around the DocumentBatchGetProxy class.  DO NOT ADD LOGIC TO THIS CLASS.
   /// This is currently excluded form unit test coverage.
   /// </summary>
   public class DocumentBatchGetProxy : IDocumentBatchGet
   {
      private readonly DocumentBatchGet underlyingObject;

      /// <inheritdoc/>
      public DocumentBatchGetProxy(DocumentBatchGet underlyingObject)
      {
         this.underlyingObject = underlyingObject;
      }

      /// <inheritdoc/>
      public List<Document> Results => this.underlyingObject.Results;

      /// <inheritdoc/>
      public bool ConsistentRead
      {
         get { return this.underlyingObject.ConsistentRead; }
         set { this.underlyingObject.ConsistentRead = value; }
      }

      /// <inheritdoc/>
      public List<string> AttributesToGet
      {
         get { return this.underlyingObject.AttributesToGet; }
         set { this.underlyingObject.AttributesToGet = value; }
      }

      /// <inheritdoc/>
      public void AddKey(Primitive hashKey, Primitive rangeKey)
      {
         this.underlyingObject.AddKey(hashKey, rangeKey);
      }

      /// <inheritdoc/>
      public void AddKey(Primitive hashKey)
      {
         this.underlyingObject.AddKey(hashKey);
      }

      /// <inheritdoc/>
      public Task ExecuteAsync()
      {
         return this.underlyingObject.ExecuteAsync();
      }
   }
}
