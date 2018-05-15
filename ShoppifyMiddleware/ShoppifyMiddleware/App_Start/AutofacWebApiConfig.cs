using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ShoppifyMiddleware.DataBase;
using System.Reflection;
using System.Data.Entity;
using ShoppifyMiddleware.Services.Setup;
using Autofac.Builder;
using ShoppifyMiddleware.Services;
using ShoppifyMiddleware;
using ShoppifyMiddleware.Controllers;

namespace ShoppifyMiddleware.App_Start
{
    public class AutofacWebapiConfig
    {

        public static IContainer Container;

        public static void Initialize(HttpConfiguration config)
        {
            var builder = RegisterServices(new ContainerBuilder());
            Initialize(config, builder);
        }


        public static void Initialize(HttpConfiguration config, IContainer container)
        {
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private static IContainer RegisterServices(ContainerBuilder builder)
        {

            //Register your Web API controllers.  

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<IDbFactory>().InstancePerRequest();

            builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IGenericRepository<>));

            builder.RegisterType(typeof(ProductService)).As(typeof(IProductService));

            
              
            Container = builder.Build();
            
            return Container;
        }

    }
}