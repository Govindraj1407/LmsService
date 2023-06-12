// <copyright file="DynamoDBWrapper.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2;
   using Amazon.DynamoDBv2.DocumentModel;
   using Amazon.DynamoDBv2.Model;
   using Microsoft.Extensions.Logging;

   /// <inheritdoc/>
   public class DynamoDBWrapper : IDynamoClient
   {
      private readonly IAmazonDynamoDB client;
      private readonly ILogger<DynamoDBWrapper> logger;
      private readonly ITableProvider tableProvider;

      /// <inheritdoc/>
      public DynamoDBWrapper(IAmazonDynamoDB client, ILogger<DynamoDBWrapper> logger, ITableProvider tableProvider)
      {
         this.client = client;
         this.logger = logger;
         this.tableProvider = tableProvider;
      }

      /// <inheritdoc />
      public async Task PutItemAsync(string tableName, Document doc)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            await table.PutItemAsync(doc);
            this.logger.LogInformation($"{nameof(Table.PutItemAsync)} on the table - {tableName} has been executed Successfully");
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.PutItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public Task<Document> GetItemAsync(string tableName, Primitive hashKey, GetItemOperationConfig config)
      {
         return GetItemAsync(tableName, hashKey, null, config);
      }

      /// <inheritdoc />
      public async Task<Document> GetItemAsync(string tableName, Primitive hashKey, Primitive rangeKey, GetItemOperationConfig config)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            Document doc;
            if (rangeKey == null)
              doc = await table.GetItemAsync(hashKey, config);
            else
               doc = await table.GetItemAsync(hashKey, rangeKey, config);

            this.logger.LogInformation($"{nameof(Table.GetItemAsync)} on the table - {tableName} has been executed Successfully");
            return doc;
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.GetItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public Task<Document> DeleteItemAsync(string tableName, Primitive hashKey)
      {
         return DeleteItemAsync(tableName, hashKey, null);
      }

      /// <inheritdoc />
      public async Task<Document> DeleteItemAsync(string tableName, Primitive hashKey, Primitive rangeKey)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            Document doc;
            if (rangeKey == null)
               doc = await table.DeleteItemAsync(hashKey);
            else
               doc = await table.DeleteItemAsync(hashKey, rangeKey);

            this.logger.LogInformation($"{nameof(Table.DeleteItemAsync)} on the table - {tableName} has been executed Successfully");
            return doc;
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.DeleteItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public async Task BatchInsertAsync(string tableName, IEnumerable<Document> documents)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            IDocumentBatchWrite batch = table.CreateBatchWrite();
            foreach (Document doc in documents)
               batch.AddDocumentToPut(doc);
            await batch.ExecuteAsync();
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.DeleteItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public async Task BatchDeleteAsync(string tableName, List<object> hashKeys, List<object> rangeKeys)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            IDocumentBatchWrite batchWrite = table.CreateBatchWrite();
            if (rangeKeys != null)
            {
               for (int i = 0; i < hashKeys.Count; i++)
               {
                  Primitive hashKey = new Primitive { Value = hashKeys[i] };
                  Primitive sortKey = new Primitive { Value = rangeKeys[i] };
                  batchWrite.AddKeyToDelete(hashKey, sortKey);
               }
            }
            else
            {
               for (int i = 0; i < hashKeys.Count; i++)
               {
                  Primitive hashKey = new Primitive { Value = hashKeys[i] };
                  batchWrite.AddKeyToDelete(hashKey);
               }
            }

            await batchWrite.ExecuteAsync();
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.DeleteItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public async Task<List<Document>> BatchGetAsync(string tableName, List<object> hashKeys, List<object> rangeKeys, bool consistentRead, IEnumerable<string> attributesToGet)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            IDocumentBatchGet batchGet = table.CreateBatchGet();

            batchGet.ConsistentRead = consistentRead;
            if (attributesToGet != null)
            {
               batchGet.AttributesToGet = attributesToGet.ToList();
            }

            if (rangeKeys != null)
            {
               for (int i = 0; i < hashKeys.Count; i++)
               {
                  Primitive hashKey = new Primitive { Value = hashKeys[i] };
                  Primitive sortKey = new Primitive { Value = rangeKeys[i] };
                  batchGet.AddKey(hashKey, sortKey);
               }
            }
            else
            {
               for (int i = 0; i < hashKeys.Count; i++)
               {
                  Primitive hashKey = new Primitive { Value = hashKeys[i] };
                  batchGet.AddKey(hashKey);
               }
            }

            await batchGet.ExecuteAsync();
            return batchGet.Results;
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.DeleteItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public async Task<List<T>> ScanAsync<T>(string tableName, ScanOperationConfig config)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            ISearch search = table.Scan(config);
            List<T> documentList = new List<T>();
            do
            {
               List<Document> currentSet = await search.GetNextSetAsync();
               foreach (Document document in currentSet)
               {
                  documentList.Add(document.ToObject<T>());
               }
            }
            while (!search.IsDone);

            return documentList;
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.DeleteItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public async Task<List<T>> QueryAsync<T>(string tableName, QueryOperationConfig config)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            ISearch search = table.Query(config);
            List<T> documentList = new List<T>();
            do
            {
               List<Document> currentSet = await search.GetNextSetAsync();
               foreach (Document document in currentSet)
               {
                  documentList.Add(document.ToObject<T>());
               }
            }
            while (!search.IsDone);

            return documentList;
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.DeleteItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public async Task<Document> UpdateItemAsync(string tableName, Document document, UpdateItemOperationConfig config)
      {
         try
         {
            ITableProxy table = await Get(tableName);
            Document result = await table.UpdateItemAsync(document, config);
            this.logger.LogInformation($"{nameof(Table.UpdateItemAsync)} on the table - {tableName} has been executed Successfully");
            return result;
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.UpdateItemAsync), tableName);
         }
      }

      /// <inheritdoc />
      public async Task UpdateItemAsync(string tableName, UpdateItemRequest request)
      {
         try
         {
            await this.client.UpdateItemAsync(request);
            this.logger.LogInformation($"{nameof(Table.UpdateItemAsync)} on the table - {tableName} has been executed Successfully");
         }
         catch (AmazonDynamoDBException ex)
         {
            throw NewCustomDynamoException(ex, nameof(Table.UpdateItemAsync), tableName);
         }
      }

      /// <summary>
      /// Loads table from Dynamo
      /// </summary>
      /// <param name="tableName">Table name to load</param>
      /// <returns>DynamoDB table</returns>
      private Task<ITableProxy> Get(string tableName)
      {
         return this.tableProvider.LoadTable(tableName);
      }

      private Exception NewCustomDynamoException(AmazonDynamoDBException ex, string action, string tableName)
      {
         string error = $"An error occured while executing the {action} operation on the table - {tableName} with the message - {ex.Message}";
         this.logger.LogError(error);

         if (ex.Message != null &&
               ex.Message.Equals("The security token included in the request is invalid."))
         {
            return new Exception("Invalid AWS Credentials.", ex);
         }
         else
         {
            return new Exception(error, ex);
         }
      }
   }
}
