// <copyright file="ITableProvider.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace DynamoDBWrapper
{
   using System.Threading.Tasks;
   using Amazon.DynamoDBv2.DocumentModel;

   /// <summary>
   /// This thin wrapper around <seealso cref="Table">Table</seealso> LoadTable method.
   /// </summary>
   public interface ITableProvider
   {
      /// <summary>
      /// Creates a Table object with the specified name, using the passed-in client to
      ///     load the table definition. The returned table will use the conversion specified
      ///     by AWSConfigs.DynamoDBConfig.ConversionSchema This method will throw an exception
      ///     if the table does not exist.
      /// </summary>
      /// <param name="tableName">The name of the table.</param>
      /// <returns>Table object representing the specified table.</returns>
      Task<ITableProxy> LoadTable(string tableName);
   }
}
