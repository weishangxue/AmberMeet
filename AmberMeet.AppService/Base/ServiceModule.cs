using AmberMeet.AppService.FileMaps;
using AmberMeet.AppService.Meets;
using AmberMeet.AppService.MeetSignfors;
using AmberMeet.AppService.Organizations;
using Autofac;

namespace AmberMeet.AppService.Base
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileMapService>().As<IFileMapService>();
            builder.RegisterType<OrgUserService>().As<IOrgUserService>();
            builder.RegisterType<MeetCommandService>().As<IMeetCommandService>();
            builder.RegisterType<MeetQueryService>().As<IMeetQueryService>();
            builder.RegisterType<MeetSignforService>().As<IMeetSignforService>();
        }
    }
}