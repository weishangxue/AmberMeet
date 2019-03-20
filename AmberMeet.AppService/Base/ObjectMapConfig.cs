namespace AmberMeet.AppService.Base
{
    public static class ObjectMapConfig
    {
        public static void Configure()
        {
            //左侧的类型是源类型(s)，右侧的类型是目标类型(t)
            //Mapper.Initialize(cfg => {
            //    cfg.CreateMap<OrgUser, OrgUserDto>()
            //        .ForMember(t => t.RoleName, opt => opt.MapFrom(s => ((OrgUserRole)s.Role).ToEnumText()))
            //        .ForMember(t => t.StatusName, opt => opt.MapFrom(s => ((OrgUserState)s.Status).ToEnumText())); });
        }
    }
}