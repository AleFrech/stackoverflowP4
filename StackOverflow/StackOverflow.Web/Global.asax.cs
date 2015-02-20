using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using AutoMapper;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutofacConfig.Register();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMaps();
        }
    }
    public class ReadOnlyRepository : IReadOnlyRepository
    {
        public Account GetById(Guid Id)
        {
            return new Account();
        }
    }

    public class ReadAndWrite : IReadAndWriteRepository
    {
        public Account GetById(Guid Id)
        {
            return new Account();
        }
    }

    public interface IReadOnlyRepository
    {
        Account GetById(Guid Id);
    }

    public interface IReadAndWriteRepository : IReadOnlyRepository
    {

    }
}
