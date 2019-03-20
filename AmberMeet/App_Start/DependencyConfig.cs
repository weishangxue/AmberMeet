using System.Web.Mvc;
using AmberMeet.AppService.Base;
using AmberMeet.Domain.Base;
using Autofac;
using Autofac.Features.ResolveAnything;
using Autofac.Integration.Mvc;

namespace AmberMeet
{
    public class DependencyConfig
    {
        public static void Configure()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.RegisterModule(new DataModule());
            builder.RegisterModule(new ServiceModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}