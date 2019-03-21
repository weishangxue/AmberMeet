using AmberMeet.Domain.Data;
using AmberMeet.Dto;
using AmberMeet.Dto.Meets;
using AutoMapper;

namespace AmberMeet.AppService.Base
{
    public static class ObjectMapConfig
    {
        public static void Configure()
        {
            //左侧的类型是源类型(s)，右侧的类型是目标类型(t)
            Mapper.Initialize(cfg =>
            {
                //org map
                cfg.CreateMap<OrgUser, OrgUserPagedDto>()
                    .ForMember(t => t.LoginName, opt => opt.MapFrom(s => s.Account));
                cfg.CreateMap<OrgUser, OrgUserDto>()
                    .ForMember(t => t.LoginName, opt => opt.MapFrom(s => s.Account));
                cfg.CreateMap<OrgUserDto, OrgUser>()
                    .ForMember(t => t.Account, opt => opt.MapFrom(s => s.LoginName));
                //meet map
                cfg.CreateMap<Meet, MeetDto>()
                    .ForMember(t => t.State, opt => opt.MapFrom(s => s.Status));
                cfg.CreateMap<Meet, MeetPagedDto>();
            });
        }
    }
}