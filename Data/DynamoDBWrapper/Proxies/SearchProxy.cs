// <copyright file="SearchProxy.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Collections.Generic;
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// Thin wrapper around the Search class.  DO NOT ADD LOGIC TO THIS CLASS.
   /// This is currently excluded form unit test coverage.
   /// </summary>
   public class SearchProxy : ISearch
   {
      private readonly Search search;

      /// <summary>
      /// Constructor.  Accepts underlying search object and uses that for the method implementations.
      /// </summary>
      /// <param name="search">The underlying object.</param>
      public SearchProxy(Search search)
      {
         this.search = search;
      }

      /// <inheritdoc/>
      public bool IsDone => this.search.IsDone;

      /// <inheritdoc/>
      public Task<List<Document>> GetNextSetAsync()
      {
         return this.search.GetNextSetAsync();
      }
   }
}
