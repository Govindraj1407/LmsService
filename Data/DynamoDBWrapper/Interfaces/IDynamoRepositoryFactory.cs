using DynamoDBWrapper;

namespace DynamoDBWrapper
{
   /// <summary>
   /// Factory for generating dynamo repositories
   /// </summary>
   public interface IDynamoRepositoryFactory
   {
      /// <summary>
      /// Gets the interface for interacting with the database
      /// </summary>
      /// <param name="tableName">The dynamo table name.</param>
      /// <param name="keyName">The key for the table.</param>      
      /// <returns>Interface representing the dynamo table.</returns>
      /// <typeparam name="Item">The db item type.</typeparam>
      /// <typeparam name="Key">The item key type.</typeparam>
      IDynamoDBRepository<Key, Item> Get<Key, Item>(string tableName, string keyName)
         where Item : new();

      /// <summary>
      /// Gets the interface for interacting with the database
      /// </summary>
      /// <param name="tableName">The dynamo table name.</param>
      /// <param name="keyName">The key for the table.</param>
      /// <param name="sortKeyName">The key for the table.</param>      
      /// <returns>Interface representing the dynamo table.</returns>
      /// <typeparam name="Item">The db item type.</typeparam>
      /// <typeparam name="Key">The item key type.</typeparam>
      /// <typeparam name="SortKey">The item sort key type.</typeparam>
      public IDynamoDBRepository<Key, Item> Get<Key, SortKey, Item>(string tableName, string keyName, string sortKeyName)
         where Item : new();
   }
}
