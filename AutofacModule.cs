// <copyright file="AutofacModule.cs" company="Trane Company">
// Copyright (c) Trane Company. All rights reserved.
// </copyright>

namespace Configurations
{
    using Autofac;
    using DynamoDBWrapper;
    using Repository;
    using System.Reflection;

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
            //builder.RegisterType(typeof(UserService)).As(typeof(IUserService));
            //builder.Register(c => new CourseWareService(
            //        c.Resolve<IDynamoRepositoryFactory>()
            //        )
            //    ).As(typeof(ICourseWareService));
            //builder.RegisterDynamoClient();
            builder.RegisterAssemblyTypes(typeof(DynamoDBRepository).GetTypeInfo().Assembly).As<DynamoDBRepository>();
            builder.RegisterAssemblyTypes(typeof(IDynamoTableConfig).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();
            builder.RegisterType(typeof(CourseWareDynamoDBRepository)).As(typeof(ICourseWareDynamoDBRepository));
        }
    }
}
