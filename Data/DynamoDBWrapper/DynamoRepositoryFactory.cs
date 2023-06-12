using Amazon;
using Amazon.DynamoDBv2;
using DynamoDBWrapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DynamoDBWrapper
{
   /// <inheritdoc/>
   public class DynamoRepositoryFactory : IDynamoRepositoryFactory
   {
      private readonly IDynamoTableConfig tableConfig;
      private readonly ILoggerFactory loggerFactory;
      private readonly DynamoDBWrapper client;

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="options">The TSMTSettings for configuring the table</param>
      /// <param name="tableConfig">The configuration of the dynamo table</param>
      /// <param name="loggerFactory">Factory for getting ILoggers</param>
      public DynamoRepositoryFactory(
         IDynamoTableConfig tableConfig,
         ILoggerFactory loggerFactory)
      {
        
         var dbConfig = new AmazonDynamoDBConfig()
         {
            RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-2")
         };

         var awsClient = new AmazonDynamoDBClient(dbConfig);
         this.client = new DynamoDBWrapper(
            awsClient,
            loggerFactory.CreateLogger<DynamoDBWrapper>(),
            new TableProvider(awsClient, loggerFactory.CreateLogger<TableProvider>()));

         this.tableConfig = tableConfig;
         this.loggerFactory = loggerFactory;
      }

      /// <inheritdoc />
      public IDynamoDBRepository<Key, Item> Get<Key, Item>(string tableName, string keyName)
         where Item : new()
      {
         this.tableConfig.TableName = tableName;
         this.tableConfig.KeyName = keyName;
         this.tableConfig.KeyType = typeof(Key);
         return new DynamoDBRepository<Key, Item>(this.tableConfig, this.loggerFactory.CreateLogger<DynamoDBRepository<Key, Item>>(), this.client);
      }

      /// <inheritdoc />
      public IDynamoDBRepository<Key, Item> Get<Key, SortKey, Item>(string tableName, string keyName, string sortKeyName)
         where Item : new()
      {
         this.tableConfig.TableName = tableName;
         this.tableConfig.KeyName = keyName;
         this.tableConfig.KeyType = typeof(Key);
         this.tableConfig.SortKeyName = sortKeyName;
         this.tableConfig.SortKeyType = typeof(SortKey);
         return new DynamoDBRepository<Key, Item>(this.tableConfig, this.loggerFactory.CreateLogger<DynamoDBRepository<Key, Item>>(), this.client);
      }
   }
}
