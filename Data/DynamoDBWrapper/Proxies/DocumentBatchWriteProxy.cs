// <copyright file="DocumentBatchWriteProxy.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// Thin wrapper around the DocumentBatchWrite class.  DO NOT ADD LOGIC TO THIS CLASS.
   /// This is currently excluded form unit test coverage.
   /// </summary>
   public class DocumentBatchWriteProxy : IDocumentBatchWrite
   {
      private readonly DocumentBatchWrite underlyingObject;

      /// <summary>
      /// Constructor.  Accepts underlying DocumentBatchWrite object and uses that for implementation.
      /// </summary>
      /// <param name="underlyingObject"></param>
      public DocumentBatchWriteProxy(DocumentBatchWrite underlyingObject)
      {
         this.underlyingObject = underlyingObject;
      }

      /// <inheritdoc/>
      public void AddDocumentToPut(Document doc)
      {
         this.underlyingObject.AddDocumentToPut(doc);
      }

      /// <inheritdoc/>
      public void AddKeyToDelete(Primitive hashKey, Primitive rangeKey)
      {
         this.underlyingObject.AddKeyToDelete(hashKey, rangeKey);
      }

      /// <inheritdoc/>
      public void AddKeyToDelete(Primitive hashKey)
      {
         this.underlyingObject.AddKeyToDelete(hashKey);
      }

      /// <inheritdoc/>
      public Task ExecuteAsync()
      {
         return this.underlyingObject.ExecuteAsync();
      }
   }
}
