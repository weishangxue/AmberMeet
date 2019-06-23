using System;
using AmberMeet.AppService.Base;
using AmberMeet.AppService.Organizations;
using AmberMeet.Domain.Base;
using AmberMeet.Domain.Data;
using AmberMeet.Domain.Organizations;
using AmberMeet.Infrastructure.Utilities;
using Autofac;
using Autofac.Features.ResolveAnything;

namespace AmberMeet.Init
{
    internal class Program
    {
        static Program()
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

        private static IOrgUserService OrgUserService { get; }

        private static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("begin...");

                if (OrgUserService.GetByAccount("admin") == null)
                {
                    //add admin user
                    var newAminUser = new OrgUser
                    {
                        Id = ConfigHelper.NewGuid,
                        Code = "adminitrator",
                        Name = "Adminitrator",
                        Account = "admin",
                        Password = ConfigHelper.DefaultUserPwd,
                        Mail = "admin@163.com",
                        Role = (int) UserRole.System,
                        Sex = (int) UserSex.Man,
                        Birthday = DateTime.Now,
                        Mobile = "-",
                        State = (int) UserState.Normal
                    };

                    OrgUserService.AddUser(newAminUser);
                    var addAdminUserMsg =
                        $"add admin user complete,loginName={newAminUser.Account},password={ConfigHelper.DefaultUserPwd}";
                    Console.WriteLine(addAdminUserMsg);
                    Console.WriteLine("continue...");
                }

                //complete
                Console.WriteLine("complete");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                LogHelper.ExceptionLog(ex);
                Console.WriteLine(ex);
                Console.ReadLine();
            }
        }
    }
}