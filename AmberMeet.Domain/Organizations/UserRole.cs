using System.ComponentModel;

namespace AmberMeet.Domain.Organizations
{
    public enum UserRole
    {
        /// <summary>
        ///     普通用户
        /// </summary>
        [Description("普通用户")] Ordinay = 1,

        /// <summary>
        ///     管理员
        /// </summary>
        [Description("管理员")] Manager = 2,

        /// <summary>
        ///     系统管理员(最高级=admin)
        /// </summary>
        [Description("系统管理员")] System = 3
    }
}