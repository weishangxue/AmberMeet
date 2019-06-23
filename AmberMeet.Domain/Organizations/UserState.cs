using System.ComponentModel;

namespace AmberMeet.Domain.Organizations
{
    /// <summary>
    ///     用户(登录账号)状态
    /// </summary>
    public enum UserState
    {
        /// <summary>
        ///     正常
        /// </summary>
        [Description("正常")] Normal = 1,

        /// <summary>
        ///     注销
        /// </summary>
        [Description("注销")] Cancle = 2

        ///// <summary>
        /////     停用(暂停)
        ///// </summary>
        //[Description("停用")] Suspend = 3
    }
}