using AmberMeet.Domain.Data;
using AmberMeet.Domain.Meets;
using AmberMeet.Domain.MeetSignfors;
using AmberMeet.Domain.Organizations;
using Autofac;

namespace AmberMeet.Domain.Base
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<FileMapRepository>().As<IFileMapRepository>();
            builder.RegisterType<OrgUserRepository>().As<IOrgUserRepository>();
            builder.RegisterType<MeetRepository>().As<IMeetRepository>();
            builder.RegisterType<MeetSignforRepository>().As<IMeetSignforRepository>();
        }
    }
}