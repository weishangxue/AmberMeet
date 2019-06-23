using System.ComponentModel;

namespace AmberMeet.Domain.Meets
{
    /// <summary>
    ///     会议状态
    /// </summary>
    public enum MeetState
    {
        /// <summary>
        ///     未激活
        /// </summary>
        [Description("未激活")] WaitActivate = 1,

        /// <summary>
        ///     激活=已确认开启且还未完成
        /// </summary>
        [Description("激活")] Activate = 2,

        /// <summary>
        ///     完成
        /// </summary>
        [Description("完成")] Complete = 3,

        /// <summary>
        ///     取消
        /// </summary>
        [Description("取消")] Cancle = 4
    }
}