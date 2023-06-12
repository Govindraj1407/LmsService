// <copyright file="DynamoDBRepository.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;
   using Amazon.DynamoDBv2.Model;
   using Microsoft.Extensions.Logging;

   /// <summary>
   /// Implementation for accessing Amazon DynamoDB for various CRUD operations such as Get, Put, Delete, etc.,
   /// </summary>
   public class DynamoDBRepository<Key, Item> : IDynamoDBRepository<Key, Item>
      where Item : new()
   {
      private readonly ILogger<DynamoDBRepository<Key, Item>> logger;

      /// <summary>
      /// The Table class is the starting object when using the Document API. It is used
      /// to Get documents from the DynamoDB table and write documents back to the DynamoDB
      /// table.
      /// </summary>
      private readonly IDynamoClient dynamoWrapper;

      /// <summary>
      /// Initializes a new DynamoDBRepository given the table config.
      /// </summary>
      /// <param name="dynamoTableConfig">AWS Dynamo table configuration such as table name, index name, etc</param>
      /// <param name="logger">Class logger for the repository.</param>
      /// <param name="dynamoWrapper">DB client wrapper</param>
      public DynamoDBRepository(IDynamoTableConfig dynamoTableConfig, ILogger<DynamoDBRepository<Key, Item>> logger, IDynamoClient dynamoWrapper)
      {
         this.DynamoTableConfig = dynamoTableConfig;
         this.logger = logger;
         this.dynamoWrapper = dynamoWrapper;
      }

      /// <summary>
      /// Gets or sets the Dynamo Table Config
      /// </summary>
      public IDynamoTableConfig DynamoTableConfig { get; set; }

      /// <summary>
      /// Initiates the asynchronous execution of the PutItem operation. Amazon.DynamoDBv2.DocumentModel.Table.PutItem
      /// </summary>
      /// <param name="tObject">Document to save</param>
      /// <returns>A collection of attribute key-value pairs that defines an inserted item in DynamoDB.</returns>
      public async Task InsertAsync(Item tObject)
      {
         await this.dynamoWrapper.PutItemAsync(this.DynamoTableConfig.TableName, tObject.ToDocument());
      }

      /// <summary>
      /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
      /// </summary>
      /// <param name="id">The partition key of an item</param>
      /// <param name="sortKey">The sort key of an item</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">List of attributes to retrieve</param>
      /// <returns>A collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      public async Task<Item> GetAsync(Key id, object sortKey = null, bool consistentRead = true, IEnumerable<string> attributesToGet = null)
      {
         GetItemOperationConfig config = new GetItemOperationConfig();
         if (attributesToGet != null)
         {
            config.AttributesToGet = attributesToGet.ToList();
         }

         config.ConsistentRead = consistentRead;

         Primitive hashKey = new Primitive { Value = id };
         if (this.DynamoTableConfig.KeyType == typeof(string))
         {
            hashKey.Type = DynamoDBEntryType.String;
         }
         else
         {
            hashKey.Type = DynamoDBEntryType.Numeric;
         }

         Primitive rangeKey = new Primitive { Value = sortKey };
         if (sortKey != null)
         {
            if (this.DynamoTableConfig.SortKeyType == typeof(string))
            {
               rangeKey.Type = DynamoDBEntryType.String;
            }
            else
            {
               rangeKey.Type = DynamoDBEntryType.Numeric;
            }
         }

         Document doc;
         if (sortKey != null)
         {
            doc = await this.dynamoWrapper.GetItemAsync(this.DynamoTableConfig.TableName, hashKey, rangeKey, config);
         }
         else
         {
            doc = await this.dynamoWrapper.GetItemAsync(this.DynamoTableConfig.TableName, hashKey, config);
         }

         return doc.ToObject<Item>();
      }

      /// <summary>
      /// Initiates the asynchronous execution of the DeleteItem operation. Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem
      /// </summary>
      /// <param name="id">The partition key of an item</param>
      /// <param name="sortKey">The sort key of an item</param>
      /// <returns>Result of the delete operation.</returns>
      public async Task DeleteAsync(Key id, object sortKey = null)
      {
         Primitive hashKey = new Primitive { Value = id };

         if (this.DynamoTableConfig.KeyType == typeof(string))
         {
            hashKey.Type = DynamoDBEntryType.String;
         }
         else
         {
            hashKey.Type = DynamoDBEntryType.Numeric;
         }

         Primitive rangeKey = new Primitive { Value = sortKey };
         if (sortKey != null)
         {
            if (this.DynamoTableConfig.SortKeyType == typeof(string))
            {
               rangeKey.Type = DynamoDBEntryType.String;
            }
            else
            {
               rangeKey.Type = DynamoDBEntryType.Numeric;
            }
         }

         if (sortKey != null)
         {
            await this.dynamoWrapper.DeleteItemAsync(this.DynamoTableConfig.TableName, hashKey, rangeKey);
         }
         else
         {
            await this.dynamoWrapper.DeleteItemAsync(this.DynamoTableConfig.TableName, hashKey);
         }

         this.logger.LogTrace("DeleteItem operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
      }

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
      /// </summary>      
      /// <param name="tObject">Attributes to update.</param>
      /// <param name="returnValues">Flag specifying what values should be returned.</param>
      /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
      public async Task<Item> PartialUpdateCommandAsync(Item tObject, ReturnValues returnValues = ReturnValues.AllNewAttributes)
      {
         Document document = tObject.ToDocument();

         UpdateItemOperationConfig config = new UpdateItemOperationConfig
         {
            ReturnValues = returnValues
         };

         var resultDocument = await this.dynamoWrapper.UpdateItemAsync(this.DynamoTableConfig.TableName, document, config);
         return resultDocument.ToObject<Item>();
      }

      /// <summary>
      /// Initiates the asynchronous execution of the Batch Insert operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
      /// </summary>      
      /// <param name="batchOfObjects">One or more documents to save.</param>
      /// <returns>Result of the batch insert</returns>
      public async Task BatchInsertAsync(IEnumerable<Item> batchOfObjects)
      {
         await this.dynamoWrapper.BatchInsertAsync(this.DynamoTableConfig.TableName, batchOfObjects.Select(o => o.ToDocument()));
         this.logger.LogTrace("Batch Insert operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
      }

      /// <summary>
      /// Initiates the asynchronous execution of the Batch Delete operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
      /// </summary>
      /// <param name="idList">List of partition keys</param>
      /// <param name="sortKeyList">List of sort keys</param>
      /// <returns>Result of the batch delete operation.</returns>
      public Task BatchDeleteAsync(IEnumerable<Key> idList, IEnumerable<object> sortKeyList = null)
      {
         List<object> keys = idList.Select(id => (object)id).ToList();
         List<object> sortKeys = sortKeyList?.ToList();
         if (sortKeys != null && keys.Count != sortKeys.Count)
         {
            throw new ArgumentException("sortKeyList not same length as idList");
         }

         return ExecuteBatchDeleteAsync(keys, sortKeys);
      }

      /// <summary>
      /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
      /// </summary>
      /// <param name="idList">List of partition keys</param>
      /// <param name="sortKeyList">List of sort keys</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">List of attributes to retrieve</param>
      /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      private Task<List<Document>> BatchGetDocumentsAsync(IEnumerable<Key> idList, IEnumerable<object> sortKeyList = null, bool consistentRead = true, IEnumerable<string> attributesToGet = null)
      {
         List<object> keys = idList.Select(id => (object)id).ToList();
         List<object> sortKeys = sortKeyList?.ToList();
         if (sortKeyList != null && keys.Count != sortKeys.Count)
         {
            throw new ArgumentException("sortKeyList not same length as idList");
         }

         return ExecuteBatchGetAsync(consistentRead, attributesToGet, keys, sortKeys);
      }

      /// <summary>
      /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
      /// </summary>
      /// <param name="idList">List of partition keys</param>
      /// <param name="sortKeyList">List of sort keys</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">List of attributes to retrieve</param>
      /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      public async Task<List<Item>> BatchGetAsync(IEnumerable<Key> idList, IEnumerable<object> sortKeyList = null, bool consistentRead = true, IEnumerable<string> attributesToGet = null)
      {
         ////Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
         List<Document> documents = await this.BatchGetDocumentsAsync(idList, sortKeyList, consistentRead, attributesToGet);
         return documents.Select(d => d.ToObject<Item>()).ToList();
      }

      /// <summary>
      /// Initiates the asynchronous execution of the GetNextSet operation. If there are
      /// more items in the Scan/Query, PaginationToken will be set and can be consumed
      /// in a new Scan/Query operation to resume retrieving items from this point.
      /// Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
      /// </summary>
      /// <param name="scanFilterConditions">Represents the selection criteria for a Query or Scan operation</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">List of attributes to retrieve</param>
      /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      public async Task<List<Item>> ScanAsync(IEnumerable<ScanFilterCondition> scanFilterConditions, bool consistentRead = true, IEnumerable<string> attributesToGet = null)
      {
         ScanOperationConfig scanOperationConfig = this.GetScanOperationConfig(scanFilterConditions, consistentRead, attributesToGet);
         List<Item> resultList = (await this.dynamoWrapper.ScanAsync<Item>(this.DynamoTableConfig.TableName, scanOperationConfig));
         this.logger.LogTrace("Scan operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
         return resultList;
      }

      /// <summary>
      /// Initiates the asynchronous execution of the GetNextSet operation. If there are
      ///     more items in the Scan/Query, PaginationToken will be set and can be consumed
      ///     in a new Scan/Query operation to resume retrieving items from this point. Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
      /// </summary>
      /// <param name="queryFilterConditions">Represents the selection criteria for a Query or Scan operation</param>
      /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
      /// <param name="attributesToGet">List of attributes to retrieve</param>
      /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
      public async Task<List<Item>> QueryAsync(IEnumerable<QueryFilterCondition> queryFilterConditions, bool consistentRead = true, IEnumerable<string> attributesToGet = null)
      {
         QueryOperationConfig config = this.GetQueryOperationConfig(queryFilterConditions, consistentRead, attributesToGet);
         List<Item> resultList = (await this.dynamoWrapper.QueryAsync<Item>(this.DynamoTableConfig.TableName, config));
         this.logger.LogInformation("Query operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
         return resultList;
      }

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation.
      /// </summary>
      /// <param name="id">The partition key of an item</param>
      /// <param name="attribute">Each attribute value is described as a name-value pair. The name is the data
      ///     type, and the value is the data itself.</param>
      /// <param name="value">Represents the data for an attribute.</param>
      /// <returns>Output of an UpdateItem operation.</returns>
      public async Task UpdateAsync(Key id, string attribute, string value)
      {
         Primitive hashKey = new Primitive { Value = id };

         if (this.DynamoTableConfig.KeyType == typeof(string))
         {
            hashKey.Type = DynamoDBEntryType.String;
         }
         else
         {
            hashKey.Type = DynamoDBEntryType.Numeric;
         }

         UpdateItemRequest request = this.GetUpdateItemRequest(hashKey, attribute, value);

         await this.dynamoWrapper.UpdateItemAsync(this.DynamoTableConfig.TableName, request);
      }

      /// <summary>
      /// Initiates the asynchronous execution of the UpdateItem operation.
      /// </summary>
      /// <param name="id">The partition key of an item</param>
      /// <param name="sortKey">The sort key of an item</param>
      /// <param name="attribute">Each attribute value is described as a name-value pair. The name is the data
      ///     type, and the value is the data itself.</param>
      /// <param name="value">Represents the data for an attribute.</param>
      /// <returns>Output of an UpdateItem operation.</returns>
      public async Task UpdateAsync(Key id, object sortKey, string attribute, string value)
      {
         Primitive hashKey = new Primitive { Value = id };
         Primitive sortId = new Primitive { Value = sortKey };

         UpdateItemRequest request = this.GetUpdateItemRequest(hashKey, sortId, attribute, value);
         await this.dynamoWrapper.UpdateItemAsync(this.DynamoTableConfig.TableName, request);
         this.logger.LogInformation("Update operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
      }

      private ScanOperationConfig GetScanOperationConfig(IEnumerable<ScanFilterCondition> scanFilterConditions, bool consistentRead = true, IEnumerable<string> attributesToGet = null)
      {
         ScanFilter scanFilter = new ScanFilter();

         foreach (ScanFilterCondition scanFilterCondition in scanFilterConditions)
         {
            if (scanFilterCondition.Type == FilterConditionType.AttributeWithoutOperator)
            {
               scanFilter.AddCondition(scanFilterCondition.AttributeName, scanFilterCondition.Condition);
            }

            if (scanFilterCondition.Type == FilterConditionType.AttributeWithOperatorAndAttributeValues)
            {
               scanFilter.AddCondition(scanFilterCondition.AttributeName, scanFilterCondition.ScanOperator, scanFilterCondition.AttributeValues);
            }

            if (scanFilterCondition.Type == FilterConditionType.AttributeWithOperatorAndValues)
            {
               scanFilter.AddCondition(scanFilterCondition.AttributeName, scanFilterCondition.ScanOperator, scanFilterCondition.DynamoDBEntry);
            }
         }

         ScanOperationConfig scanOperationConfig = new ScanOperationConfig()
         {
            ConsistentRead = consistentRead,
            Filter = scanFilter
         };
         if (attributesToGet != null)
         {
            scanOperationConfig.AttributesToGet = attributesToGet.ToList();
         }

         if (!string.IsNullOrEmpty(this.DynamoTableConfig.IndexName))
         {
            scanOperationConfig.IndexName = this.DynamoTableConfig.IndexName;
         }

         return scanOperationConfig;
      }

      private QueryOperationConfig GetQueryOperationConfig(IEnumerable<QueryFilterCondition> queryFilterConditions, bool consistentRead = true, IEnumerable<string> attributesToGet = null)
      {
         QueryFilter queryFilter = new QueryFilter();

         foreach (QueryFilterCondition queryFilterCondition in queryFilterConditions)
         {
            if (queryFilterCondition.Type == FilterConditionType.AttributeWithoutOperator)
            {
               queryFilter.AddCondition(queryFilterCondition.AttributeName, queryFilterCondition.Condition);
            }

            if (queryFilterCondition.Type == FilterConditionType.AttributeWithOperatorAndAttributeValues)
            {
               queryFilter.AddCondition(queryFilterCondition.AttributeName, queryFilterCondition.QueryOperator, queryFilterCondition.AttributeValues);
            }

            if (queryFilterCondition.Type == FilterConditionType.AttributeWithOperatorAndValues)
            {
               queryFilter.AddCondition(queryFilterCondition.AttributeName, queryFilterCondition.QueryOperator, queryFilterCondition.DynamoDBEntry);
            }
         }

         QueryOperationConfig queryOperationConfig = new QueryOperationConfig()
         {
            ConsistentRead = consistentRead,
            Filter = queryFilter
         };
         if (attributesToGet != null)
         {
            queryOperationConfig.AttributesToGet = attributesToGet.ToList();
         }

         if (!string.IsNullOrEmpty(this.DynamoTableConfig.IndexName))
         {
            queryOperationConfig.IndexName = this.DynamoTableConfig.IndexName;
         }

         return queryOperationConfig;
      }

      private UpdateItemRequest GetUpdateItemRequest(Primitive id, string attribute, string value)
      {
         AttributeValue keyAttributeValue;
         if (this.DynamoTableConfig.KeyType == typeof(string))
         {
            keyAttributeValue = new AttributeValue { S = id };
         }
         else
         {
            keyAttributeValue = new AttributeValue { N = id };
         }

         return new UpdateItemRequest
         {
            TableName = this.DynamoTableConfig.TableName,
            Key = new Dictionary<string, AttributeValue>
                {
                    { this.DynamoTableConfig.KeyName, keyAttributeValue }
                },
            AttributeUpdates = new Dictionary<string, AttributeValueUpdate>()
                {
                    {
                        attribute,
                        new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = value } }
                    },
                },
         };
      }

      private UpdateItemRequest GetUpdateItemRequest(Primitive id, Primitive sortKey, string attribute, string value)
      {
         AttributeValue keyAttributeValue;
         if (this.DynamoTableConfig.KeyType == typeof(string))
         {
            keyAttributeValue = new AttributeValue { S = id };
         }
         else
         {
            keyAttributeValue = new AttributeValue { N = id };
         }

         AttributeValue sortKeyAttributeValue;
         if (this.DynamoTableConfig.SortKeyType == typeof(string))
         {
            sortKeyAttributeValue = new AttributeValue { S = sortKey };
         }
         else
         {
            sortKeyAttributeValue = new AttributeValue { N = sortKey };
         }

         return new UpdateItemRequest
         {
            TableName = this.DynamoTableConfig.TableName,
            Key = new Dictionary<string, AttributeValue>
                {
                    { this.DynamoTableConfig.KeyName, keyAttributeValue },
                    { this.DynamoTableConfig.SortKeyName,  sortKeyAttributeValue }
                },
            AttributeUpdates = new Dictionary<string, AttributeValueUpdate>()
                {
                    {
                        attribute,
                        new AttributeValueUpdate { Action = "PUT", Value = new AttributeValue { S = value } }
                    },
                },
         };
      }

      private async Task ExecuteBatchDeleteAsync(List<object> keys, List<object> sortKeys)
      {
         await this.dynamoWrapper.BatchDeleteAsync(this.DynamoTableConfig.TableName, keys, sortKeys);
         this.logger.LogTrace("Batch Delete operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
      }

      private async Task<List<Document>> ExecuteBatchGetAsync(bool consistentRead, IEnumerable<string> attributesToGet, List<object> keys, List<object> sortKeys)
      {
         List<Document> documents = await this.dynamoWrapper.BatchGetAsync(this.DynamoTableConfig.TableName, keys, sortKeys, consistentRead, attributesToGet);
         this.logger.LogTrace("Batch Get operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
         return documents;
      }
   }
}
