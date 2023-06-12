// <copyright file="AutofacModule.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace Configurations
{
    using Autofac;
    using DynamoDBWrapper;
    using global::Services;

    /// <summary>
    /// Module for configuring dependency injection
    /// </summary>
    public class AutofacModule : Autofac.Module
   {

      /// <summary>
      /// Initializes a new instance of the <see cref="AutofacModule"/> class.
      /// </summary>
      public AutofacModule()
      {
      }

      /// <summary>
      /// Load the dependencies
      /// </summary>
      /// <param name="builder">builder</param>
      protected override void Load(ContainerBuilder builder)
      {
            builder.RegisterType(typeof(UserService)).As(typeof(IUserService));
            builder.Register(c => new CourseWareService(
                    c.Resolve<IDynamoRepositoryFactory>()
                    )
                ).As(typeof(ICourseWareService));
            //builder.RegisterType(typeof(DynamoDBRepository<string, User>)).As(typeof(IDynamoDBRepository<string, User>));
            //builder.RegisterType(typeof(DynamoDBRepository<int, Course>)).As(typeof(IDynamoDBRepository<int, Course>));
            //builder.RegisterType(typeof(DynamoRepositoryFactory)).As(typeof(IDynamoRepositoryFactory));
            builder.RegisterDynamoClient();
            //builder.RegisterType<DocumentDBConnectionFactory>()
            //    .AsImplementedInterfaces()
            //    .WithParameter("connectionString", "test")
            //    .SingleInstance();
            //builder.RegisterType(typeof(UserDocumentDbRepository)).As(typeof(IUserDocumentDbRepository));
        }
    }
}
