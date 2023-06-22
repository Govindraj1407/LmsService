// <copyright file="DynamoDBRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Amazon;
    using Amazon.DynamoDBv2;
    using Amazon.DynamoDBv2.DocumentModel;
    using Amazon.DynamoDBv2.Model;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implementation for accessing Amazon DynamoDB for various CRUD operations such as Get, Put, Delete, etc.,
    /// </summary>
    public class DynamoDBRepository : IDynamoDBRepository
    {
        private readonly ILogger<DynamoDBRepository> logger;

        /// <summary>
        /// Implementation for accessing Amazon DynamoDB
        /// </summary>
        private readonly IAmazonDynamoDB client;

        /// <summary>
        /// The Table class is the starting object when using the Document API. It is used
        /// to Get documents from the DynamoDB table and write documents back to the DynamoDB
        /// table.
        /// </summary>
        private Table dynamoDBTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamoDBRepository"/> class.
        /// </summary>
        /// <param name="client">Dynamo DB client</param>
        /// <param name="dynamoTableConfig">AWS Dynamo table configuration such as table name, index name, etc</param>
        /// <param name="logger">logger</param>
        public DynamoDBRepository(IAmazonDynamoDB client, IDynamoTableConfig dynamoTableConfig, ILogger<DynamoDBRepository> logger)
        {
            this.client = client;
            this.DynamoTableConfig = dynamoTableConfig;
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the Dynamo Table Config
        /// </summary>
        public IDynamoTableConfig DynamoTableConfig { get; set; }

        /// <summary>
        /// Gets or sets the region information used to lazily compute the service endpoints.
        /// </summary>
        private RegionEndpoint RegionEndpoint { get; set; }

        /// <summary>
        /// Initiates the asynchronous execution of the PutItem operation. Amazon.DynamoDBv2.DocumentModel.Table.PutItem
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="tObject">Document to save</param>
        /// <returns>A collection of attribute key-value pairs that defines an inserted item in DynamoDB.</returns>
        public async Task InsertAsync<T>(T tObject)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the PutItem operation", this.DynamoTableConfig.TableName);
                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

                var document = tObject.ToDocument();
                await this.dynamoDBTable.PutItemAsync(document);

                this.logger.LogInformation("PutItem operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the PutItem operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when inserting an item", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
        /// </summary>
        /// <param name="id">The partition key of an item</param>
        /// <param name="sortKey">The sort key of an item</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>A collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        public async Task<Document> GetAsync(object id, object sortKey = null, bool consistentRead = true, List<string> attributesToGet = null)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);

                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the GetItem operation", this.DynamoTableConfig.TableName);

                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

                GetItemOperationConfig config = new GetItemOperationConfig();
                if (attributesToGet != null)
                {
                    config.AttributesToGet = attributesToGet;
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

                if (sortKey != null)
                {
                    return await this.dynamoDBTable.GetItemAsync(hashKey, rangeKey, config);
                }
                else
                {
                    return await this.dynamoDBTable.GetItemAsync(hashKey, config);
                }
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the GetItem operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when getting an item", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetItem operation. Amazon.DynamoDBv2.DocumentModel.Table.GetItem
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="id">The partition key of an item</param>
        /// <param name="sortKey">The sort key of an item</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>A collection of attribute key-value pairs that defines the retrieved item in DynamoDB.</returns>
        public async Task<T> GetAsync<T>(object id, object sortKey = null, bool consistentRead = true, List<string> attributesToGet = null)
        {
            try
            {
                var document = await this.GetAsync(id, sortKey, consistentRead, attributesToGet);
                return document.ToObject<T>();
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the GetItem operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when getting an item", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the DeleteItem operation. Amazon.DynamoDBv2.DocumentModel.Table.DeleteItem
        /// </summary>
        /// <param name="id">The partition key of an item</param>
        /// <param name="sortKey">The sort key of an item</param>
        /// <returns>Result of the delete operation.</returns>
        public async Task DeleteAsync(object id, object sortKey = null)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);

                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the DeleteItem operation", this.DynamoTableConfig.TableName);

                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

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
                    await this.dynamoDBTable.DeleteItemAsync(hashKey, rangeKey);
                }
                else
                {
                    await this.dynamoDBTable.DeleteItemAsync(hashKey);
                }

                this.logger.LogInformation("DeleteItem operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);

                return;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the DeleteItem operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when deleting an item", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation. Amazon.DynamoDBv2.DocumentModel.Table.UpdateItem
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="tObject">Attributes to update.</param>
        /// <param name="returnValues">Flag specifying what values should be returned.</param>
        /// <returns>A Task that can be used to poll or wait for results, or both.</returns>
        public async Task<Document> PartialUpdateCommandAsync<T>(T tObject, ReturnValues returnValues = ReturnValues.AllNewAttributes)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the UpdateItem operation", this.DynamoTableConfig.TableName);

                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

                var document = tObject.ToDocument();

                UpdateItemOperationConfig config = new UpdateItemOperationConfig
                {
                    ReturnValues = returnValues
                };

                var result = await this.dynamoDBTable.UpdateItemAsync(document, config);

                return result;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogInformation("UpdateItem operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when updating an item", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Batch Insert operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="batchOfObjects">One or more documents to save.</param>
        /// <returns>Result of the batch insert</returns>
        public async Task BatchInsertAsync<T>(List<T> batchOfObjects)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the Batch Insert operation", this.DynamoTableConfig.TableName);

                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

                ////Creates a DocumentBatchWrite object for the current table, allowing a batch-put/delete
                ////     operation against DynamoDB.
                var batchWrite = this.dynamoDBTable.CreateBatchWrite();

                foreach (var tObject in batchOfObjects)
                {
                    var document = tObject.ToDocument();
                    ////Add a single Document to put.
                    batchWrite.AddDocumentToPut(document);
                }

                ////Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
                await batchWrite.ExecuteAsync();

                this.logger.LogInformation("Batch Insert operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the Batch Insert operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when inserting item(s)", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Batch Delete operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
        /// </summary>
        /// <param name="idList">List of partition keys</param>
        /// <param name="sortKeyList">List of sort keys</param>
        /// <returns>Result of the batch delete operation.</returns>
        public async Task BatchDeleteAsync(List<object> idList, List<object> sortKeyList = null)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the Batch Delete operation", this.DynamoTableConfig.TableName);

                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

                if (sortKeyList != null && idList.Count != sortKeyList.Count)
                {
                    throw new ArgumentException("sortKeyList not same length as idList");
                }

                /////Creates a DocumentBatchWrite object for the current table, allowing a batch-put/delete
                ////     operation against DynamoDB.
                var batchWrite = this.dynamoDBTable.CreateBatchWrite();

                if (sortKeyList != null)
                {
                    for (int i = 0; i < idList.Count; i++)
                    {
                        Primitive hashKey = new Primitive { Value = idList[i] };
                        Primitive sortKey = new Primitive { Value = sortKeyList[i] };

                        ////Add a single item to delete, identified by its hash-and-range primary key.
                        batchWrite.AddKeyToDelete(hashKey, sortKey);
                    }
                }
                else
                {
                    for (int i = 0; i < idList.Count; i++)
                    {
                        Primitive hashKey = new Primitive { Value = idList[i] };
                        ////Add a single item to delete, identified by its hash-and-range primary key.
                        batchWrite.AddKeyToDelete(hashKey);
                    }
                }

                /////Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchWrite.Execute
                await batchWrite.ExecuteAsync();

                this.logger.LogInformation("Batch Delete operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the Batch Delete operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when deleting item(s)", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
        /// </summary>
        /// <param name="idList">List of partition keys</param>
        /// <param name="sortKeyList">List of sort keys</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        public async Task<List<Document>> BatchGetAsync(List<object> idList, List<object> sortKeyList = null, bool consistentRead = true, List<string> attributesToGet = null)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the Batch Get operation", this.DynamoTableConfig.TableName);

                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

                if (sortKeyList != null && idList.Count != sortKeyList.Count)
                {
                    throw new ArgumentException("sortKeyList not same length as idList");
                }

                ////Creates a DocumentBatchGet object for the current table, allowing a batch-get
                ////     operation against DynamoDB.
                var batchGet = this.dynamoDBTable.CreateBatchGet();

                ////If set to true, a consistent read is issued. Otherwise eventually-consistent
                ////     is used.
                batchGet.ConsistentRead = consistentRead;
                if (attributesToGet != null)
                {
                    batchGet.AttributesToGet = attributesToGet;
                }

                if (sortKeyList != null)
                {
                    for (int i = 0; i < idList.Count; i++)
                    {
                        Primitive hashKey = new Primitive { Value = idList[i] };
                        Primitive sortKey = new Primitive { Value = sortKeyList[i] };
                        ////Add a single item to get, identified by its hash-and-range primary key.
                        batchGet.AddKey(hashKey, sortKey);
                    }
                }
                else
                {
                    for (int i = 0; i < idList.Count; i++)
                    {
                        Primitive hashKey = new Primitive { Value = idList[i] };
                        ////Add a single item to get, identified by its hash key.
                        batchGet.AddKey(hashKey);
                    }
                }

                ////Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
                await batchGet.ExecuteAsync();

                this.logger.LogInformation("Batch Get operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);

                ////List of results retrieved from DynamoDB. Populated after Execute is called.
                return batchGet.Results;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the Batch Get operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when getting item(s)", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="idList">List of partition keys</param>
        /// <param name="sortKeyList">List of sort keys</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        public async Task<List<T>> BatchGetAsync<T>(List<object> idList, List<object> sortKeyList = null, bool consistentRead = true, List<string> attributesToGet = null)
            where T : new()
        {
            try
            {
                List<T> tList = new List<T>();

                ////Initiates the asynchronous execution of the Execute operation. Amazon.DynamoDBv2.DocumentModel.DocumentBatchGet.Execute
                var documents = await this.BatchGetAsync(idList, sortKeyList, consistentRead, attributesToGet);

                foreach (var document in documents)
                {
                    T t = new T();
                    t = document.ToObject<T>();
                    tList.Add(t);
                }

                return tList;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the Batch Get operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when getting item(s)", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetNextSet operation. If there are
        /// more items in the Scan/Query, PaginationToken will be set and can be consumed
        /// in a new Scan/Query operation to resume retrieving items from this point.
        /// Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="scanFilterConditions">Represents the selection criteria for a Query or Scan operation</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        public async Task<List<T>> ScanAsync<T>(List<ScanFilterCondition> scanFilterConditions, bool consistentRead = true, List<string> attributesToGet = null)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the Scan operation", this.DynamoTableConfig.TableName);

                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

                ScanOperationConfig scanOperationConfig = this.GetScanOperationConfig(scanFilterConditions, consistentRead, attributesToGet);

                ////Initiates a Search object to Scan a DynamoDB table, with the specified config.
                ////     No calls are made until the Search object is used.
                Search search = this.dynamoDBTable.Scan(scanOperationConfig);

                List<T> resultList = new List<T>();
                List<Document> documentList = new List<Document>();
                do
                {
                    //// Initiates the asynchronous execution of the GetNextSet operation. If there are
                    ////     more items in the Scan/Query, PaginationToken will be set and can be consumed
                    ////     in a new Scan/Query operation to resume retrieving items from this point.
                    ////Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
                    documentList = await search.GetNextSetAsync();
                    foreach (var document in documentList)
                    {
                        resultList.Add(document.ToObject<T>());
                    }
                }
                while (!search.IsDone);

                this.logger.LogInformation("Scan operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);

                return resultList;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the Scan operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when scanning item(s)", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the GetNextSet operation. If there are
        ///     more items in the Scan/Query, PaginationToken will be set and can be consumed
        ///     in a new Scan/Query operation to resume retrieving items from this point. Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
        /// </summary>
        /// <typeparam name="T">A collection of attribute key-value pairs that defines an item in DynamoDB.</typeparam>
        /// <param name="queryFilterConditions">Represents the selection criteria for a Query or Scan operation</param>
        /// <param name="consistentRead">If set to true, this flag ensures that the most recently written data is returned.</param>
        /// <param name="attributesToGet">List of attributes to retrieve</param>
        /// <returns>List of collection of attribute key-value pairs that defines an item in DynamoDB.</returns>
        public async Task<List<T>> QueryAsync<T>(List<QueryFilterCondition> queryFilterConditions, bool consistentRead = true, List<string> attributesToGet = null)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the Query operation", this.DynamoTableConfig.TableName);

                this.dynamoDBTable = Table.LoadTable(this.client, this.DynamoTableConfig.TableName);

                QueryOperationConfig queryOperationConfig = this.GetQueryOperationConfig(queryFilterConditions, consistentRead, attributesToGet);

                ////Initiates a Search object to Query a DynamoDB table, with the specified config.
                ////     No calls are made until the Search object is used.
                Search search = this.dynamoDBTable.Query(queryOperationConfig);

                List<T> resultList = new List<T>();
                List<Document> documentList = new List<Document>();
                do
                {
                    ////Initiates the asynchronous execution of the GetNextSet operation. If there are
                    ////     more items in the Scan/Query, PaginationToken will be set and can be consumed
                    ////     in a new Scan/Query operation to resume retrieving items from this point.
                    ////Amazon.DynamoDBv2.DocumentModel.Search.GetNextSet
                    documentList = await search.GetNextSetAsync();
                    foreach (var document in documentList)
                    {
                        resultList.Add(document.ToObject<T>());
                    }
                }
                while (!search.IsDone);

                this.logger.LogInformation("Query operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);

                return resultList;
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the Query operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when querying item(s)", ex.Message));
                }
            }
        }

        /// <summary>
        /// Initiates the asynchronous execution of the UpdateItem operation.
        /// </summary>
        /// <param name="id">The partition key of an item</param>
        /// <param name="attribute">Each attribute value is described as a name-value pair. The name is the data
        ///     type, and the value is the data itself.</param>
        /// <param name="value">Represents the data for an attribute.</param>
        /// <returns>Output of an UpdateItem operation.</returns>
        public async Task UpdateAsync(object id, string attribute, string value)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the Update operation", this.DynamoTableConfig.TableName);

                Primitive hashKey = new Primitive { Value = id };

                if (this.DynamoTableConfig.KeyType == typeof(string))
                {
                    hashKey.Type = DynamoDBEntryType.String;
                }
                else
                {
                    hashKey.Type = DynamoDBEntryType.Numeric;
                }

                var request = this.GetUpdateItemRequest(hashKey, attribute, value);

                await this.client.UpdateItemAsync(request);

                this.logger.LogInformation("Update operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the Update  item operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when updating an item", ex.Message));
                }
            }
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
        public async Task UpdateAsync(object id, object sortKey, string attribute, string value)
        {
            try
            {
                this.logger.LogInformation("Verifying whether the given table - {0} is existing in AWS", this.DynamoTableConfig.TableName);
                var tableExists = await this.TableExistsAsync(this.DynamoTableConfig.TableName);
                if (!tableExists)
                {
                    throw new Exception("Specified table does n't exist");
                }

                this.logger.LogInformation("Specified table - {0} is existing in AWS, hence proceeding with the Update operation", this.DynamoTableConfig.TableName);

                Primitive hashKey = new Primitive { Value = id };
                Primitive sortId = new Primitive { Value = sortKey };

                var request = this.GetUpdateItemRequest(hashKey, sortId, attribute, value);

                await this.client.UpdateItemAsync(request);

                this.logger.LogInformation("Update operation on the table - {0} has been executed Successfully", this.DynamoTableConfig.TableName);
            }
            catch (AmazonDynamoDBException ex)
            {
                this.logger.LogError("An error occured while executing the Update  item operation on the table - {0} with the message - {1}", this.DynamoTableConfig.TableName, ex);

                if (ex.Message != null &&
                    ex.Message.Equals("The security token included in the request is invalid."))
                {
                    throw new Exception("Invalid AWS Credentials.");
                }
                else
                {
                    throw new Exception(string.Format("An error occurred with the message '{0}' when updating an item", ex.Message));
                }
            }
        }

        private ScanOperationConfig GetScanOperationConfig(List<ScanFilterCondition> scanFilterConditions, bool consistentRead = true, List<string> attributesToGet = null)
        {
            ScanFilter scanFilter = new ScanFilter();

            foreach (var scanFilterCondition in scanFilterConditions)
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
                scanOperationConfig.AttributesToGet = attributesToGet;
            }

            if (!string.IsNullOrEmpty(this.DynamoTableConfig.IndexName))
            {
                scanOperationConfig.IndexName = this.DynamoTableConfig.IndexName;
            }

            return scanOperationConfig;
        }

        private QueryOperationConfig GetQueryOperationConfig(List<QueryFilterCondition> queryFilterConditions, bool consistentRead = true, List<string> attributesToGet = null)
        {
            QueryFilter queryFilter = new QueryFilter();

            foreach (var queryFilterCondition in queryFilterConditions)
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
                queryOperationConfig.AttributesToGet = attributesToGet;
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

        private async Task<bool> TableExistsAsync(string tableName)
        {
            try
            {
                var res = await this.client.DescribeTableAsync(new DescribeTableRequest
                {
                    TableName = tableName
                });

                if (res == null || res.Table == null)
                {
                    return false;
                }

                var statusCode = res.HttpStatusCode;
                var ready = res.Table.TableStatus;

                return ready == TableStatus.ACTIVE;
            }
            catch (AmazonDynamoDBException ex)
            {
                if (ex.ErrorCode != null && ex.ErrorCode.Equals("ResourceNotFoundException"))
                {
                    return false;
                }

                throw ex;
            }
        }
    }
}
