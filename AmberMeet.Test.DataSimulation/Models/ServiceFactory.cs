using AmberMeet.AppService.Base;
using AmberMeet.AppService.Organizations;
using AmberMeet.Domain.Base;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace AmberMeet.Test.DataSimulation.Models
{
    public class ServiceFactory
    {
        static ServiceFactory()
        {
            ObjectMapConfig.Configure();
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            containerBuilder.RegisterModule(new DataModule());
            containerBuilder.RegisterModule(new ServiceModule());
            using (var container = containerBuilder.Build())
            {
                OrgUserService = container.Resolve<IOrgUserService>();
            }
        }

        public static IOrgUserService OrgUserService { get; }
    }
}