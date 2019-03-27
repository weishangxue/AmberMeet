using AmberMeet.Domain.Data;
using AmberMeet.Dto.Meets;
using AmberMeet.Dto.MeetSignfors;
using AmberMeet.Dto.Organizations;
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
                cfg.CreateMap<OrgUser, OrgUserDto>()
                    .ForMember(t => t.LoginName, opt => opt.MapFrom(s => s.Account));
                cfg.CreateMap<OrgUserDto, OrgUser>()
                    .ForMember(t => t.Account, opt => opt.MapFrom(s => s.LoginName));
                cfg.CreateMap<OrgUser, OrgUserPagedDto>()
                    .ForMember(t => t.LoginName, opt => opt.MapFrom(s => s.Account));
                //meet map
                cfg.CreateMap<Meet, MeetDto>()
                    .ForMember(t => t.State, opt => opt.MapFrom(s => s.Status));
                cfg.CreateMap<MeetDto, Meet>()
                    .ForMember(t => t.Status, opt => opt.MapFrom(s => s.State));
                cfg.CreateMap<Meet, MeetWaitActivatePagedDto>();
                cfg.CreateMap<Meet, MeetPagedDto>()
                    .ForMember(t => t.StartTime,
                        opt => opt.MapFrom(s => s.MeetActivate == null ? s.StartTime : s.MeetActivate.StartTime))
                    .ForMember(t => t.EndTime,
                        opt => opt.MapFrom(s => s.MeetActivate == null ? s.EndTime : s.MeetActivate.EndTime))
                    .ForMember(t => t.Place,
                        opt => opt.MapFrom(s => s.MeetActivate == null ? s.Place : s.MeetActivate.Place));
                //meet signfor map
                cfg.CreateMap<MeetSignfor, MeetSignforDto>()
                    .ForMember(t => t.State, opt => opt.MapFrom(s => s.Status))
                    .ForMember(t => t.Subject, opt => opt.MapFrom(s => s.Meet.Subject))
                    .ForMember(t => t.Body, opt => opt.MapFrom(s => s.Meet.Body))
                    .ForMember(t => t.Place, opt => opt.MapFrom(s => s.Meet.Place))
                    .ForMember(t => t.NeedFeedback, opt => opt.MapFrom(s => s.Meet.NeedFeedback))
                    .ForMember(t => t.StartTime, opt => opt.MapFrom(s => s.Meet.StartTime))
                    .ForMember(t => t.EndTime, opt => opt.MapFrom(s => s.Meet.EndTime))
                    .ForMember(t => t.SignorName, opt => opt.MapFrom(s => s.OrgUser.Name));
            });
        }
    }
}