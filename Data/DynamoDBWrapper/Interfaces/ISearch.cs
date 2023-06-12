// <copyright file="ISearch.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// This thin wrapper around <seealso cref="Search">Search</seealso> class from the AWS DynamoDB SDK
   /// </summary>
   public interface ISearch
   {
      /// <summary>
      /// Flag that, if true, indicates that the search is done
      /// </summary>
      bool IsDone { get; }

      /// <summary>
      /// Initiates the asynchronous execution of the GetNextSet operation. If there are
      ///     more items in the Scan/Query, PaginationToken will be set and can be consumed
      ///     in a new Scan/Query operation to resume retrieving items from this point. Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
      /// </summary>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      Task<List<Document>> GetNextSetAsync();
   }
}
