using System.Reflection;
using Autofac;

namespace DynamoDBWrapper
{
   /// <summary>
   /// Extensions used for registering for autofac.
   /// </summary>
   public static class AutofacExtensions
   {
      /// <summary>
      /// Registers everything for dependency injection.
      /// </summary>
      /// <param name="builder">Used to register dependencies.</param>
      public static void RegisterDynamoClient(this ContainerBuilder builder)
      {
         builder.RegisterAssemblyTypes(typeof(IDynamoTableConfig).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();
      }
   }
}
