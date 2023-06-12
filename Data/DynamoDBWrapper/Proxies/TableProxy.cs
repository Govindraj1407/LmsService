// <copyright file="TableProxy.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// Thin wrapper around the Table class.  DO NOT ADD LOGIC TO THIS CLASS.
   /// This is currently excluded form unit test coverage.
   /// </summary>
   public class TableProxy : ITableProxy
   {
      private readonly Table table;

      /// <summary>
      /// Constructor.  Saves the passed in table and exposes methods that pass thru to it.
      /// </summary>
      /// <param name="underlyingTable">The underlying dynamodb table.</param>
      public TableProxy(Table underlyingTable)
      {
         this.table = underlyingTable;
      }

      /// <summary>
      /// This method is here to facilitate testing...can't figure out how to mock the right things for this static class.
      /// </summary>
      /// <param name="client">The DB client.</param>
      /// <param name="tableName">The target table name.</param>
      /// <returns></returns>
      public ITableProxy LoadTable(IAmazonDynamoDB client, string tableName)
      {
         return (TableProxy)Table.LoadTable(client, tableName, true);
      }

      /// <inheritdoc/>
      public Task<Document> UpdateItemAsync(Document document, UpdateItemOperationConfig config)
      {
         return this.table.UpdateItemAsync(document, config);
      }

      /// <inheritdoc/>
      public ISearch Query(QueryOperationConfig config)
      {
         return new SearchProxy(this.table.Query(config));
      }

      /// <inheritdoc/>
      public ISearch Scan(ScanOperationConfig config)
      {
         return new SearchProxy(this.table.Scan(config));
      }

      /// <inheritdoc/>
      public IDocumentBatchGet CreateBatchGet()
      {
         return new DocumentBatchGetProxy(this.table.CreateBatchGet());
      }

      /// <inheritdoc/>
      public IDocumentBatchWrite CreateBatchWrite()
      {
         return new DocumentBatchWriteProxy(this.table.CreateBatchWrite());
      }

      /// <inheritdoc/>
      public Task<Document> DeleteItemAsync(Primitive hashKey, Primitive rangeKey)
      {
         return this.table.DeleteItemAsync(hashKey, rangeKey);
      }

      /// <inheritdoc/>
      public Task<Document> DeleteItemAsync(Primitive hashKey)
      {
         return this.table.DeleteItemAsync(hashKey);
      }

      /// <inheritdoc/>
      public Task PutItemAsync(Document doc)
      {
         return this.table.PutItemAsync(doc);
      }

      /// <inheritdoc/>
      public Task<Document> GetItemAsync(Primitive hashKey, Primitive rangeKey, GetItemOperationConfig config)
      {
         return this.table.GetItemAsync(hashKey, rangeKey, config);
      }

      /// <inheritdoc/>
      public Task<Document> GetItemAsync(Primitive hashKey, GetItemOperationConfig config)
      {
         return this.table.GetItemAsync(hashKey, config);
      }

      /// <summary>
      /// Defines cast operator for converting a ITableProxy to a Table
      /// </summary>
      /// <param name="tableProxy">The table proxy to convert from</param>
      public static implicit operator Table(TableProxy tableProxy) => tableProxy.table;

      /// <summary>
      /// Defines cast operator for converting a Table to a TableProxy
      /// </summary>
      /// <param name="table">The table to convert from</param>
      public static explicit operator TableProxy(Table table) => new TableProxy(table);
   }
}
