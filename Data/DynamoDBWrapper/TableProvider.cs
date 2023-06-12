// <copyright file="TableProvider.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System;
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2;
   using Amazon.DynamoDBv2.DocumentModel;
   using Amazon.DynamoDBv2.Model;
   using Microsoft.Extensions.Logging;

   /// <summary>
   /// Factory for creating 
   /// </summary>
   public class TableProvider : ITableProvider
   {
      private ITableProxy table;
      private readonly IAmazonDynamoDB client;
      private readonly ILogger logger;

      /// <summary>
      /// Constructor.
      /// </summary>
      /// <param name="client"></param>
      /// <param name="logger"></param>
      public TableProvider(IAmazonDynamoDB client, ILogger logger)
      {
         this.table = null;
         this.client = client;
         this.logger = logger;
      }

      /// <summary>
      /// Verifies the specified table exists and then loads it.  Throws <see cref="ArgumentException">ArgumentException</see> if table does not exist. 
      /// </summary>      
      /// <param name="tableName"></param>
      /// <returns>Task with the LoadTable result.</returns>
      public async Task<ITableProxy> LoadTable(string tableName)
      {
         if (this.table == null)
         {
            this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", tableName);
            bool tableExists = await this.TableExistsAsync(tableName);
            if (!tableExists)
            {
               throw new System.ArgumentException($"Table [{tableName}] does not exist!");
            }

            this.table = new TableProxy(LoadTable(client, tableName));
         }

         return this.table;
      }

      /// <summary>
      /// This method is here to facilitate testing...can't figure out how to mock the right things for this static class.
      /// </summary>
      /// <param name="client">The DB client.</param>
      /// <param name="tableName">The target table name.</param>
      /// <returns></returns>
      protected virtual Table LoadTable(IAmazonDynamoDB client, string tableName)
      {
         return Table.LoadTable(client, tableName, true);
      }

      private async Task<bool> TableExistsAsync(string tableName)
      {
         try
         {
            DescribeTableResponse res = await this.client.DescribeTableAsync(new DescribeTableRequest
            {
               TableName = tableName
            });

            if (res == null || res.Table == null)
            {
               return false;
            }

            TableStatus ready = res.Table.TableStatus;

            return ready == TableStatus.ACTIVE;
         }
         catch (AmazonDynamoDBException ex)
         {
            if (ex.ErrorCode != null && ex.ErrorCode.Equals("ResourceNotFoundException"))
            {
               return false;
            }

            throw;
         }
      }
   }
}
