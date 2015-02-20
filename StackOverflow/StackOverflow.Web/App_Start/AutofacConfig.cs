using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;

namespace StackOverflow.Web
{
    public static class AutofacConfig
    {
        public static void Register()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(MvcApplication).Assembly);
            builder.RegisterFilterProvider();
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterType<ReadOnlyRepository>().As<IReadOnlyRepository>();

            builder.Register(c => Mapper.Engine).As<IMappingEngine>();

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}